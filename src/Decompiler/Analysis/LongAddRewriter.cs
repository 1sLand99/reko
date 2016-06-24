﻿#region License
/* 
 * Copyright (C) 1999-2016 John Källén.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using Reko.Core;
using Reko.Core.Code;
using Reko.Core.Lib;
using Reko.Core.Expressions;
using Reko.Core.Machine;
using Reko.Core.Operators;
using Reko.Core.Types;
using Reko.Evaluation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Reko.Analysis
{
    /// <summary>
    /// Locates instances of add aLo, bLow followed later adc aHi, bHi and
    /// merges them into (add a, b).
    /// </summary>
    /// <remarks>
    /// Limitations: only does this on pairs within the same basic block,
    /// as dominator analysis and SSA analysis haven't been done this early. 
    /// //$TODO: consider doing this _after_ SSA, so that we reap the benefit
    /// of performing this across basic block boundaries. The challenge is
    /// to introduce new variables xx_yy that interfere with existing xx 
    /// and yy references.
    /// This code must be run immediately after SSA translation. In particular
    /// it must happen before value propagation since VP changes 
    /// <code>
    /// adc r1,0,C
    /// </code>
    /// to
    /// <code>
    /// add r1,C
    /// </code>
    /// </remarks>
    [Obsolete]
    public class LongAddRewriter
    {
        private Procedure proc;
        private Expression dst;
        private IProcessorArchitecture arch;

        private static InstructionMatcher adcPattern;
        private static InstructionMatcher addPattern;
        private static ExpressionMatcher memOffset;
        private static ExpressionMatcher segMemOffset;
        private static InstructionMatcher condm;

        public LongAddRewriter(Procedure proc, IProcessorArchitecture arch)
        {
            this.arch = arch;
            this.proc = proc;
        }

        static LongAddRewriter()
        {
            condm = new InstructionMatcher(
                new Assignment(
                    ExpressionMatcher.AnyId("grf"),
                    new ConditionOf(
                        ExpressionMatcher.AnyExpression("exp"))));

            addPattern = new InstructionMatcher(
                new Assignment(
                    ExpressionMatcher.AnyId("dst"),
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op"), 
                        VoidType.Instance,
                        ExpressionMatcher.AnyExpression("left"),
                        ExpressionMatcher.AnyExpression("right"))));

            adcPattern = new InstructionMatcher(
                new Assignment(
                    ExpressionMatcher.AnyId("dst"),
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op1"),
                        VoidType.Instance,
                        new BinaryExpression(
                            ExpressionMatcher.AnyOperator("op2"),
                            VoidType.Instance,
                            ExpressionMatcher.AnyExpression("left"),
                            ExpressionMatcher.AnyExpression("right")),
                        ExpressionMatcher.AnyExpression("cf"))));

            memOffset = new ExpressionMatcher(
                new MemoryAccess(
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op"),
                        VoidType.Instance,
                        ExpressionMatcher.AnyExpression("base"),
                        ExpressionMatcher.AnyConstant("offset")),
                    ExpressionMatcher.AnyDataType("dt")));

            segMemOffset = new ExpressionMatcher(
                new SegmentedAccess(
                    null,
                    ExpressionMatcher.AnyId(),
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op"),
                        VoidType.Instance,
                        ExpressionMatcher.AnyExpression("base"),
                        ExpressionMatcher.AnyConstant("offset")),
                    ExpressionMatcher.AnyDataType("dt")));
        }

        public Instruction CreateLongInstruction(AddSubCandidate loCandidate, AddSubCandidate hiCandidate)
        {
            var totalSize = PrimitiveType.Create(
                Domain.SignedInt | Domain.UnsignedInt,
                loCandidate.Dst.DataType.Size + loCandidate.Dst.DataType.Size);
            var left = CreateCandidate(loCandidate.Left, hiCandidate.Left, totalSize);
            var right = CreateCandidate(loCandidate.Right, hiCandidate.Right, totalSize);
            this.dst = CreateCandidate(loCandidate.Dst, hiCandidate.Dst, totalSize);

            if (left == null || right == null || dst == null)
                return null;
        
            var expSum = new BinaryExpression(loCandidate.Op, left.DataType, left, right);
            var idDst = dst as Identifier;
            if (idDst != null)
            {
                return new Assignment(idDst, expSum);
            }
            else
            {
                return new Store(dst, expSum);
            }
        }

        public void Transform()
        {
            foreach (var block in proc.ControlGraph.Blocks)
            {
                ReplaceLongAdditions(block);
            }
            proc.Dump(true);
        }

        public void ReplaceLongAdditions(Block block)
        {
            var listIstmsToKill = new List<int>();
            for (int i = 0; i < block.Statements.Count; ++i)
            {
                var loInstr = MatchAddSub(block.Statements[i].Instruction);
                if (loInstr == null)
                    continue;
                var cond = FindConditionOf(block.Statements, i, loInstr.Dst);
                if (cond == null)
                    continue;

                var hiInstr = FindUsingInstruction(block.Statements, cond.StatementIndex, loInstr);
                if (hiInstr == null)
                    continue;

                var longInstr = CreateLongInstruction(loInstr, hiInstr);
                if (longInstr != null)
                {
                    listIstmsToKill.Add(loInstr.StatementIndex);
                    block.Statements[hiInstr.StatementIndex].Instruction = longInstr;
                    cond = FindConditionOf(block.Statements, hiInstr.StatementIndex, hiInstr.Dst);
                    if (cond != null)
                    {
                        block.Statements[cond.StatementIndex].Instruction =
                            new Assignment(
                                cond.FlagGroup, new ConditionOf(dst));
                        i = cond.StatementIndex;
                    }
                }
            }
            foreach (var i in Enumerable.Reverse(listIstmsToKill))
            {
                block.Statements.RemoveAt(i);
            }
        }

        /// <summary>
        /// Determines if the carry flag reaches a using instruction.
        /// </summary>
        /// <param name="instrs"></param>
        /// <param name="i"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public AddSubCandidate FindUsingInstruction(StatementList stms, int i, AddSubCandidate loInstr)
        {
            for (++i; i < stms.Count; ++i)
            {
                var asc = MatchAdcSbc(stms[i].Instruction);
                if (asc != null)
                {
                    Debug.Print("Left sides: [{0}] [{1}]", asc.Left, loInstr.Left);
                    if (asc.Left.GetType() != loInstr.Left.GetType())
                        return null;
                    asc.StatementIndex = i;
                    return asc;
                }
                var ass = stms[i].Instruction as Assignment;
                if (ass == null)
                    continue;
                if (IsCarryFlag(ass.Dst))
                    return null;
            }
            return null;
        }

        public int FindUsingInstruction2(StatementList stms, int i, Operator next)
        {
            for (++i; i < stms.Count; ++i)
            {
                var ass = stms[i].Instruction as Assignment;
                if (ass == null)
                    continue;
                var bin = ass.Src as BinaryExpression;
                if (bin == null)
                    continue;
                if (bin.Operator == next)
                    return i;
                if (IsCarryFlag(ass.Dst))
                    return -1;
            }
            return -1;
        }

        public bool IsCarryFlag(Expression exp)
        {
            var cf = exp as Identifier;
            if (cf == null)
                return false;
            var grf = cf.Storage as FlagGroupStorage;
            if (grf == null)
                return false;
            return (arch.CarryFlagMask & grf.FlagGroupBits) != 0;
        }

        /// <summary>
        /// Finds the subsequent statement in this block that defines a condition code based on the
        /// result in expression <paramref name="exp"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ax"></param>
        /// <returns></returns>
        public CondMatch FindConditionOf(StatementList stms, int iStm, Expression exp)
        {
            for (int i = iStm + 1; i < stms.Count; ++i)
            {
                if (!condm.Match(stms[i].Instruction))
                    continue;
                var grf = (Identifier) condm.CapturedExpressions("grf");
                var condExp = condm.CapturedExpressions("exp");
                if (grf.Storage is FlagGroupStorage && exp == condExp)
                {
                    return new CondMatch { FlagGroup = grf, src = exp, StatementIndex = i };
                }
            }
            return null;
        }

        private Expression CreateCandidate(Expression expLo, Expression expHi, DataType totalSize)
        {
            var idLo = expLo as Identifier;
            var idHi = expHi as Identifier;
            if (idLo != null && idHi != null)
            {
                return proc.Frame.EnsureSequence(idHi.Storage, idLo.Storage, totalSize);
            }
            var memDstLo = expLo as MemoryAccess;
            var memDstHi = expHi as MemoryAccess;
            if (memDstLo != null && memDstHi != null && MemoryOperandsAdjacent(memDstLo, memDstHi))
            {
                return CreateMemoryAccess(memDstLo, totalSize);
            }
            var immLo = expLo as Constant;
            var immHi = expHi as Constant;
            if (immLo != null && immHi != null)
            {
                return Constant.Create(totalSize, ((ulong)immHi.ToUInt32() << expLo.DataType.BitSize) | immLo.ToUInt32());
            }
            return null;
        }

        private Expression CreateMemoryAccess(MemoryAccess mem, DataType totalSize)
        {
            var segmem = mem as SegmentedAccess;
            if (segmem != null)
            {
                return new SegmentedAccess(segmem.MemoryId, segmem.BasePointer, segmem.EffectiveAddress, totalSize);
            }
            else
            {
                return new MemoryAccess(mem.MemoryId, mem.EffectiveAddress, totalSize);
            }
        }

        public class CondMatch
        {
            public int StatementIndex;
            public Identifier FlagGroup;
            public Expression src;
        }

        public CondMatch MatchCond(Instruction instr)
        {
            var ass = instr as Assignment;
            if (ass == null)
                return null;
            var cond = ass.Src as ConditionOf;
            if (cond == null)
                return null;
            var src = cond.Expression as Identifier;
            if (cond == null)
                return null;
            return new CondMatch { src = src, FlagGroup = ass.Dst };
        }

        public bool MemoryOperandsAdjacent(MemoryAccess m1, MemoryAccess m2)
        {
            //$TODO: endianness?
            var off1 = GetOffset(m1);
            var off2 = GetOffset(m2);
            if (off1 == null || off2 == null)
                return false;
            return off1.ToInt32() + m1.DataType.Size == off2.ToInt32();
        }

        private Constant GetOffset(MemoryAccess access)
        {
            if (memOffset.Match(access))
            {
                return (Constant) memOffset.CapturedExpression("offset");
            }
            if (segMemOffset.Match(access))
            {
                return (Constant)segMemOffset.CapturedExpression("offset");
            }
            return null;
        }

        /// <summary>
        /// Matches an "ADC" or "SBB/SBC" pattern.
        /// </summary>
        /// <param name="instr"></param>
        /// <returns>If the match succeeded, returns a partial BinaryExpression
        /// with the left and right side of the ADC/SBC instruction.</returns>
        public AddSubCandidate MatchAdcSbc(Instruction instr)
        {
            if (!adcPattern.Match(instr))
                return null;
            if (!IsCarryFlag(adcPattern.CapturedExpressions("cf")))
                return null;
            var op = adcPattern.CapturedOperators("op2");
            if (!IsIAddOrISub(op))
                return null;
            return new AddSubCandidate
            {
                Dst = adcPattern.CapturedExpressions("dst"),
                Op = op,
                Left = adcPattern.CapturedExpressions("left"),
                Right = adcPattern.CapturedExpressions("right")
            };
        }

        public AddSubCandidate MatchAddSub(Instruction instr)
        {
            if (!addPattern.Match(instr))
                return null;
            var op = addPattern.CapturedOperators("op");
            if (!IsIAddOrISub(op))
                return null;
            return new AddSubCandidate
            {
                Dst = addPattern.CapturedExpressions("dst"),
                Op = op,
                Left = addPattern.CapturedExpressions("left"),
                Right = addPattern.CapturedExpressions("right")
            };
        }

        private static bool IsIAddOrISub(Operator op)
        {
            return (op == Operator.IAdd || op == Operator.ISub);
        }

        public IEnumerable<CarryLinkedInstructions> FindCarryLinkedInstructions(Block block)
        {
            for (var i = block.Statements.Count - 1; i>= 0; --i) 
            {
                //FindUseCarryFlagInAddition(stm);
                //FindDefOfCarry(stm);

            }
            yield break;
        }

        public static void TestCondition()
        {
            //LongAddRewriter larw = new LongAddRewriter(this.frame, state);
            //int iUse = larw.IndexOfUsingOpcode(instrs, i, next);
            //if (iUse >= 0 && larw.Match(instrCur, instrs[iUse]))
            //{
            //    instrs[iUse].code = Opcode.nop;
            //    larw.EmitInstruction(op, emitter);
            //    return larw.Dst;
            //}
        }

    }

    public class LongAddRewriter2
    {
        private SsaState ssa;
        private Expression dst;
        private IProcessorArchitecture arch;

        private static InstructionMatcher adcPattern;
        private static InstructionMatcher addPattern;
        private static ExpressionMatcher memOffset;
        private static ExpressionMatcher segMemOffset;
        private static InstructionMatcher condm;

        public LongAddRewriter2(IProcessorArchitecture arch, SsaState ssa)
        {
            this.ssa = ssa;
            this.arch = arch;
        }

        static LongAddRewriter2()
        {
            condm = new InstructionMatcher(
                new Assignment(
                    ExpressionMatcher.AnyId("grf"),
                    new ConditionOf(
                        ExpressionMatcher.AnyExpression("exp"))));

            addPattern = new InstructionMatcher(
                new Assignment(
                    ExpressionMatcher.AnyId("dst"),
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op"),
                        VoidType.Instance,
                        ExpressionMatcher.AnyExpression("left"),
                        ExpressionMatcher.AnyExpression("right"))));

            adcPattern = new InstructionMatcher(
                new Assignment(
                    ExpressionMatcher.AnyId("dst"),
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op1"),
                        VoidType.Instance,
                        new BinaryExpression(
                            ExpressionMatcher.AnyOperator("op2"),
                            VoidType.Instance,
                            ExpressionMatcher.AnyExpression("left"),
                            ExpressionMatcher.AnyExpression("right")),
                        ExpressionMatcher.AnyExpression("cf"))));

            memOffset = new ExpressionMatcher(
                new MemoryAccess(
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op"),
                        VoidType.Instance,
                        ExpressionMatcher.AnyExpression("base"),
                        ExpressionMatcher.AnyConstant("offset")),
                    ExpressionMatcher.AnyDataType("dt")));

            segMemOffset = new ExpressionMatcher(
                new SegmentedAccess(
                    null,
                    ExpressionMatcher.AnyId(),
                    new BinaryExpression(
                        ExpressionMatcher.AnyOperator("op"),
                        VoidType.Instance,
                        ExpressionMatcher.AnyExpression("base"),
                        ExpressionMatcher.AnyConstant("offset")),
                    ExpressionMatcher.AnyDataType("dt")));
        }

        public void CreateLongInstruction(AddSubCandidate loCandidate, AddSubCandidate hiCandidate)
        {
            var totalSize = PrimitiveType.Create(
                Domain.SignedInt | Domain.UnsignedInt,
                loCandidate.Dst.DataType.Size + loCandidate.Dst.DataType.Size);
            var left = CreateCandidate(loCandidate.Left, hiCandidate.Left, totalSize);
            var right = CreateCandidate(loCandidate.Right, hiCandidate.Right, totalSize);
            this.dst = CreateCandidate(loCandidate.Dst, hiCandidate.Dst, totalSize);
            var stmts = hiCandidate.Statement.Block.Statements;
            var linAddr = hiCandidate.Statement.LinearAddress;
            var iStm = stmts.IndexOf(hiCandidate.Statement);

            var stmMkLeft = stmts.Insert(
                iStm++,
                linAddr,
                CreateMkSeq(left, hiCandidate.Left, loCandidate.Left));
            left = ReplaceDstWithSsaIdentifier(left, null, stmMkLeft);

            var stmMkRight = stmts.Insert(
                iStm++,
                linAddr,
                CreateMkSeq(right, hiCandidate.Right, loCandidate.Right));
            right = ReplaceDstWithSsaIdentifier(right, null, stmMkRight);

            var expSum = new BinaryExpression(loCandidate.Op, left.DataType, left, right);
            Instruction instr = Assign(dst, expSum);
            var stmLong = stmts.Insert(iStm++, linAddr, instr);
            this.dst = ReplaceDstWithSsaIdentifier(this.dst, expSum, stmLong);

            var sidDst = GetSsaIdentifierOf(dst);
            var sidLeft = GetSsaIdentifierOf(left);
            var sidRight = GetSsaIdentifierOf(right);
            if (sidLeft != null)
            {
                GetSsaIdentifierOf(loCandidate.Left).Uses.Add(stmMkLeft);
                GetSsaIdentifierOf(hiCandidate.Left).Uses.Add(stmMkLeft);
            }
            if (sidRight != null)
            {
                GetSsaIdentifierOf(loCandidate.Right).Uses.Add(stmMkRight);
                GetSsaIdentifierOf(hiCandidate.Right).Uses.Add(stmMkRight);
            }
            if (sidDst != null)
            {
                sidLeft.Uses.Add(stmLong);
                sidRight.Uses.Add(stmLong);
            }

            var sidDstLo = GetSsaIdentifierOf(loCandidate.Dst);
            if (sidDstLo != null)
            {

                var cast = new Cast(loCandidate.Dst.DataType, dst);
                var stmCastLo = stmts.Insert(iStm++, linAddr, new Assignment(
                    sidDstLo.Identifier, cast));
                var stmDeadLo = sidDstLo.DefStatement;
                sidDstLo.DefExpression = cast;
                sidDstLo.DefStatement = stmCastLo;

                var sidDstHi = GetSsaIdentifierOf(hiCandidate.Dst);
                var slice = new Slice(hiCandidate.Dst.DataType, dst, (uint)loCandidate.Dst.DataType.BitSize);
                var stmSliceHi = stmts.Insert(iStm++, linAddr, new Assignment(
                    sidDstHi.Identifier, slice));
                var stmDeadHi = sidDstHi.DefStatement;
                sidDstHi.DefExpression = slice;
                sidDstHi.DefStatement = stmSliceHi;
                if (sidDstLo != null)
                {
                    sidDstLo.Uses.Add(stmLong);
                }
                if (sidDstHi != null)
                {
                    sidDstHi.Uses.Add(stmLong);
                }
                ssa.DeleteStatement(stmDeadLo);
                ssa.DeleteStatement(stmDeadHi);
            }
        }

        private Expression ReplaceDstWithSsaIdentifier(Expression dst, BinaryExpression src, Statement stmLong)
        {
            var ass = stmLong.Instruction as Assignment;
            if (ass != null) {
                var sid = ssa.Identifiers.Add(ass.Dst, stmLong, src, false);
                ass.Dst = sid.Identifier;
                return ass.Dst;
            }
            return dst;
        }

        private Instruction CreateMkSeq(Expression dst, Expression hi, Expression lo)
        {
            return Assign(dst, new MkSequence(dst.DataType, hi, lo));
        }

        private Instruction Assign(Expression dst, Expression src)
        {
            var idDst = dst as Identifier;
            if (idDst != null)
            {
                return new Assignment(idDst, src);
            }
            else
            {
                return new Store(dst, src);
            }

        }
        private SsaIdentifier GetSsaIdentifierOf(Expression dst)
        {
            var id = dst as Identifier;
            if (id == null)
                return null;
            else
                return ssa.Identifiers[id];
        }

        public void Transform()
        {
            foreach (var block in ssa.Procedure.ControlGraph.Blocks)
            {
                ReplaceLongAdditions(block);
            }
        }

        public void ReplaceLongAdditions(Block block)
        {
            var stmtsOrig = block.Statements.ToList();
            for (int i = 0; i < block.Statements.Count; ++i)
            {
                var loInstr = MatchAddSub(block.Statements[i]);
                if (loInstr == null)
                    continue;
                var cond = FindConditionOf(stmtsOrig, i, loInstr.Dst);
                if (cond == null)
                    continue;

                var hiInstr = FindUsingInstruction(cond.FlagGroup, loInstr);
                if (hiInstr == null)
                    continue;

                CreateLongInstruction(loInstr, hiInstr);
            }
        }

        /// <summary>
        /// Determines if the carry flag reaches a using instruction.
        /// </summary>
        /// <param name="instrs"></param>
        /// <param name="i"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public AddSubCandidate FindUsingInstruction(Identifier cy, AddSubCandidate loInstr)
        {
            foreach (var use in ssa.Identifiers[cy].Uses)
            {
                var asc = MatchAdcSbc(use);
                if (asc != null)
                {
                    Debug.Print("Left sides: [{0}] [{1}]", asc.Left, loInstr.Left);
                    if (asc.Left.GetType() != loInstr.Left.GetType())
                        return null;
                    asc.Statement = use;
                    return asc;
                }
                var ass = use.Instruction as Assignment;
                if (ass == null)
                    continue;
                if (IsCarryFlag(ass.Dst))
                    return null;
            }
            return null;
        }

        public bool IsCarryFlag(Expression exp)
        {
            var cf = exp as Identifier;
            if (cf == null)
                return false;
            var grf = cf.Storage as FlagGroupStorage;
            if (grf == null)
                return false;
            return (arch.CarryFlagMask & grf.FlagGroupBits) != 0;
        }

        /// <summary>
        /// Finds the subsequent statement in this block that defines a condition code based on the
        /// result in expression <paramref name="exp"/>.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="ax"></param>
        /// <returns></returns>
        public CondMatch FindConditionOf(List<Statement> stms, int iStm, Expression exp)
        {
            var idLo = exp as Identifier;
            if (idLo != null)
            {
                foreach (var use in ssa.Identifiers[idLo].Uses.Where(u => condm.Match(u.Instruction)))
                {
                    var grf = (Identifier)condm.CapturedExpressions("grf");
                    var condExp = condm.CapturedExpressions("exp");
                    if (grf.Storage is FlagGroupStorage && exp == condExp)
                    {
                        return new CondMatch { FlagGroup = grf, src = exp, Statement = use };
                    }
                }
            }
            for (int i = iStm + 1; i < stms.Count; ++i)
            {
                if (!condm.Match(stms[i].Instruction))
                    continue;
                var grf = (Identifier)condm.CapturedExpressions("grf");
                var condExp = condm.CapturedExpressions("exp");
                if (grf.Storage is FlagGroupStorage && exp == condExp)
                {
                    return new CondMatch { FlagGroup = grf, src = exp, StatementIndex = i };
                }
            }
            return null;
        }

        private Expression CreateCandidate(Expression expLo, Expression expHi, DataType totalSize)
        {
            var idLo = expLo as Identifier;
            var idHi = expHi as Identifier;
            if (idLo != null && idHi != null)
            {
                return ssa.Procedure.Frame.EnsureSequence(idHi.Storage, idLo.Storage, totalSize);
            }
            var memDstLo = expLo as MemoryAccess;
            var memDstHi = expHi as MemoryAccess;
            if (memDstLo != null && memDstHi != null && MemoryOperandsAdjacent(memDstLo, memDstHi))
            {
                return CreateMemoryAccess(memDstLo, totalSize);
            }
            var immLo = expLo as Constant;
            var immHi = expHi as Constant;
            if (immLo != null && immHi != null)
            {
                return Constant.Create(totalSize, ((ulong)immHi.ToUInt32() << expLo.DataType.BitSize) | immLo.ToUInt32());
            }
            throw new NotImplementedException();
        }

        private Expression CreateMemoryAccess(MemoryAccess mem, DataType totalSize)
        {
            var segmem = mem as SegmentedAccess;
            if (segmem != null)
            {
                return new SegmentedAccess(segmem.MemoryId, segmem.BasePointer, segmem.EffectiveAddress, totalSize);
            }
            else
            {
                return new MemoryAccess(mem.MemoryId, mem.EffectiveAddress, totalSize);
            }
        }

        public class CondMatch
        {
            public int StatementIndex;
            public Identifier FlagGroup;
            public Expression src;
            public Statement Statement;
        }

        public bool MemoryOperandsAdjacent(MemoryAccess m1, MemoryAccess m2)
        {
            //$TODO: endianness?
            var off1 = GetOffset(m1);
            var off2 = GetOffset(m2);
            if (off1 == null || off2 == null)
                return false;
            return off1.ToInt32() + m1.DataType.Size == off2.ToInt32();
        }

        private Constant GetOffset(MemoryAccess access)
        {
            if (memOffset.Match(access))
            {
                return (Constant)memOffset.CapturedExpression("offset");
            }
            if (segMemOffset.Match(access))
            {
                return (Constant)segMemOffset.CapturedExpression("offset");
            }
            return null;
        }

        /// <summary>
        /// Matches an "ADC" or "SBB/SBC" pattern.
        /// </summary>
        /// <param name="instr"></param>
        /// <returns>If the match succeeded, returns a partial BinaryExpression
        /// with the left and right side of the ADC/SBC instruction.</returns>
        public AddSubCandidate MatchAdcSbc(Statement stm)
        {
            if (!adcPattern.Match(stm.Instruction))
                return null;
            if (!IsCarryFlag(adcPattern.CapturedExpressions("cf")))
                return null;
            var op = adcPattern.CapturedOperators("op2");
            if (!IsIAddOrISub(op))
                return null;
            return new AddSubCandidate
            {
                Dst = adcPattern.CapturedExpressions("dst"),
                Op = op,
                Left = adcPattern.CapturedExpressions("left"),
                Right = adcPattern.CapturedExpressions("right"),
                Statement = stm
            };
        }

        public AddSubCandidate MatchAddSub(Statement stm)
        {
            if (!addPattern.Match(stm.Instruction))
                return null;
            var op = addPattern.CapturedOperators("op");
            if (!IsIAddOrISub(op))
                return null;
            return new AddSubCandidate
            {
                Dst = addPattern.CapturedExpressions("dst"),
                Op = op,
                Left = addPattern.CapturedExpressions("left"),
                Right = addPattern.CapturedExpressions("right")
            };
        }

        private static bool IsIAddOrISub(Operator op)
        {
            return (op == Operator.IAdd || op == Operator.ISub);
        }

        public IEnumerable<CarryLinkedInstructions> FindCarryLinkedInstructions(Block block)
        {
            for (var i = block.Statements.Count - 1; i >= 0; --i)
            {
                //FindUseCarryFlagInAddition(stm);
                //FindDefOfCarry(stm);

            }
            yield break;
        }

        public static void TestCondition()
        {
            //LongAddRewriter larw = new LongAddRewriter(this.frame, state);
            //int iUse = larw.IndexOfUsingOpcode(instrs, i, next);
            //if (iUse >= 0 && larw.Match(instrCur, instrs[iUse]))
            //{
            //    instrs[iUse].code = Opcode.nop;
            //    larw.EmitInstruction(op, emitter);
            //    return larw.Dst;
            //}
        }

    }

    public class AddSubCandidate
    {
        public int StatementIndex;
        public Statement Statement;
        public Expression Dst;
        public Operator Op;
        public Expression Left;
        public Expression Right;
    }

    public class CarryLinkedInstructions
    {
        public Instruction High;
        public Instruction Low;
    }
}
