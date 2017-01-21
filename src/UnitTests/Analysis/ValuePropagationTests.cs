#region License
/* 
 * Copyright (C) 1999-2017 John K�ll�n.
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

using Reko.Analysis;
using Reko.Evaluation;
using Reko.Core;
using Reko.Core.Code;
using Reko.Core.Expressions;
using Reko.Core.Operators;
using Reko.Core.Types;
using Reko.Core.Machine;
using Reko.UnitTests.Mocks;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using Rhino.Mocks;
using System.Diagnostics;
using System.Collections.Generic;
using Reko.UnitTests.Fragments;

namespace Reko.UnitTests.Analysis
{
	[TestFixture]
	public class ValuePropagationTests : AnalysisTestBase
	{
		SsaIdentifierCollection ssaIds;
        private MockRepository mr;
        private IProcessorArchitecture arch;
        private Program program;
        private IImportResolver importResolver;
        private FakeDecompilerEventListener listener;

        [SetUp]
		public void Setup()
		{
			ssaIds = new SsaIdentifierCollection();
            mr = new MockRepository();
            arch = mr.Stub<IProcessorArchitecture>();
            importResolver = mr.Stub<IImportResolver>();
            listener = new FakeDecompilerEventListener();
            program = new Program()
            {
                Architecture = arch,
            };
        }

        private Identifier Reg32(string name)
        {
            var mr = new RegisterStorage(name, ssaIds.Count, 0, PrimitiveType.Word32);
            Identifier id = new Identifier(mr.Name, mr.DataType, mr);
            SsaIdentifier sid = new SsaIdentifier(id, id, null, null, false);
            ssaIds.Add(id, sid);
            return sid.Identifier;
        }

        private Identifier Reg16(string name)
        {
            var mr = new RegisterStorage(name, ssaIds.Count, 0, PrimitiveType.Word16);
            Identifier id = new Identifier(mr.Name, mr.DataType, mr);
            SsaIdentifier sid = new SsaIdentifier(id, id, null, null, false);
            ssaIds.Add(id, sid);
            return sid.Identifier;
        }

        private Identifier Reg8(string name)
        {
            var mr = new RegisterStorage(name, ssaIds.Count, 0, PrimitiveType.Byte);
            Identifier id = new Identifier(mr.Name, mr.DataType, mr);
            SsaIdentifier sid = new SsaIdentifier(id, id, null, null, false);
            ssaIds.Add(id, sid);
            return sid.Identifier;
        }

        private ExternalProcedure CreateExternalProcedure(string name, Identifier ret, params Identifier[] parameters)
        {
            var ep = new ExternalProcedure(name, new FunctionType(ret, parameters));
            ep.Signature.ReturnAddressOnStack = 4;
            return ep;
        }

        private Identifier RegArg(int n, string name)
        {
            return new Identifier(
                name,
                PrimitiveType.Word32,
                new RegisterStorage(name, n, 0, PrimitiveType.Word32));
        }

        private Identifier StackArg(int offset)
        {
            return new Identifier(
                string.Format("arg{0:X2}", offset),
                PrimitiveType.Word32,
                new StackArgumentStorage(offset, PrimitiveType.Word32));
        }

		protected override void RunTest(Program program, TextWriter writer)
		{
			var dfa = new DataFlowAnalysis(program, importResolver, new FakeDecompilerEventListener());
			foreach (Procedure proc in program.Procedures.Values)
			{
				writer.WriteLine("= {0} ========================", proc.Name);
                SsaTransform sst = new SsaTransform(
                    program, 
                    proc, 
                    new HashSet<Procedure>(),
                    importResolver, 
                    dfa.ProgramDataFlow);
                sst.Transform();
				SsaState ssa = sst.SsaState;
                var cce = new ConditionCodeEliminator(ssa, program.Platform);
                cce.Transform();
				ssa.Write(writer);
				proc.Write(false, writer);
				writer.WriteLine();

                ValuePropagator vp = new ValuePropagator(program.Architecture, ssa, listener);
				vp.Transform();
                sst.RenameFrameAccesses = true;
                sst.Transform();

				ssa.Write(writer);
				proc.Write(false, writer);
			}
		}

        private SsaState RunTest(ProcedureBuilder m)
        {
            var proc = m.Procedure;
            var gr = proc.CreateBlockDominatorGraph();
            var sst = new SsaTransform(
                program,
                proc,
                new HashSet<Procedure>(),
                importResolver,
                new ProgramDataFlow());
            var ssa = sst.SsaState;
            sst.Transform();

            var vp = new ValuePropagator(arch, ssa, listener);
            vp.Transform();
            return ssa;
        }

        private void AssertStringsEqual(string sExp, SsaState ssa)
        {
            var sw = new StringWriter();
            ssa.Write(sw);
            ssa.Procedure.Write(false, sw);
            var sActual = sw.ToString();
            if (sExp != sActual)
            {
                Debug.Print("{0}", sActual);
                Assert.AreEqual(sExp, sActual);
            }
        }

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpChainTest()
		{
			RunFileTest_x86_real("Fragments/multiple/chaincalls.asm", "Analysis/VpChainTest.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpConstPropagation()
		{
			RunFileTest_x86_real("Fragments/constpropagation.asm", "Analysis/VpConstPropagation.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpGlobalHandle()
		{
            Given_FakeWin32Platform(mr);
            this.platform.Stub(p => p.LookupGlobalByName(null, null)).IgnoreArguments().Return(null);
            this.platform.Stub(p => p.DataTypeFromImportName(null)).IgnoreArguments().Return(null);
            mr.ReplayAll();
			RunFileTest_x86_32("Fragments/import32/GlobalHandle.asm", "Analysis/VpGlobalHandle.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpNegsNots()
		{
			RunFileTest_x86_real("Fragments/negsnots.asm", "Analysis/VpNegsNots.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpNestedRepeats()
		{
			RunFileTest_x86_real("Fragments/nested_repeats.asm", "Analysis/VpNestedRepeats.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpSuccessiveDecs()
		{
			RunFileTest_x86_real("Fragments/multiple/successivedecs.asm", "Analysis/VpSuccessiveDecs.txt");
		}

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpWhileGoto()
		{
			RunFileTest_x86_real("Fragments/while_goto.asm", "Analysis/VpWhileGoto.txt");
		}

        [Test]
        [Ignore(Categories.AnalysisDevelopment)]
        public void VpReg00011()
        {
            RunFileTest_x86_real("Fragments/regressions/r00011.asm", "Analysis/VpReg00011.txt");
        }

        [Test]
        [Category(Categories.IntegrationTests)]
        public void VpLoop()
        {
            var b = new ProgramBuilder();
            b.Add("main", m =>
            {
                var r = m.Reg32("r0", 0);
                var zf = m.Flags("Z");
                m.Label("l0000");
                m.Store(r, 0);
                m.Assign(r, m.ISub(r, 4));
                m.Assign(zf, m.Cond(r));
                m.BranchIf(m.Test(ConditionCode.NE, zf), "l0000");

                m.Label("l0001");
                m.Assign(r, 42);

                m.Label("l0002");
                m.Store(r, 12);
                m.Assign(r, m.ISub(r, 4));
                m.BranchIf(m.Eq0(r), "l0002");

                m.Return();
            });
            RunFileTest(b.BuildProgram(), "Analysis/VpLoop.txt");
        }

		[Test]
        [Category(Categories.IntegrationTests)]
		public void VpDbp()
		{
			Procedure proc = new DpbFragment().Procedure;
            var sst = new SsaTransform(
                program,
                proc,
                new HashSet<Procedure>(),
                importResolver, 
                new ProgramDataFlow());
            sst.Transform();
            SsaState ssa = sst.SsaState;

			ValuePropagator vp = new ValuePropagator(arch, ssa, listener);
			vp.Transform();

			using (FileUnitTester fut = new FileUnitTester("Analysis/VpDbp.txt"))
			{
				proc.Write(false, fut.TextWriter);
				fut.TextWriter.WriteLine();
				fut.AssertFilesEqual();
			}
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpEquality()
		{
			Identifier foo = Reg32("foo");

            var vp = CreatePropagatorWithDummyStatement();
			BinaryExpression expr = 
				new BinaryExpression(Operator.Eq, PrimitiveType.Bool, 
				new BinaryExpression(Operator.ISub, PrimitiveType.Word32, foo,
				Constant.Word32(1)),
				Constant.Word32(0));
			Assert.AreEqual("foo - 0x00000001 == 0x00000000", expr.ToString());

			Expression simpler = vp.VisitBinaryExpression(expr);
			Assert.AreEqual("foo == 0x00000001", simpler.ToString());
		}

        private ExpressionSimplifier CreatePropagatorWithDummyStatement()
        {
            var ctx = new SsaEvaluationContext(arch, ssaIds);
            ctx.Statement = new Statement(0, new SideEffect(Constant.Word32(32)), null);
            return new ExpressionSimplifier(ctx, listener);
        }

		[Test]
        [Category(Categories.UnitTests)]
		public void VpAddZero()
		{
			Identifier r = Reg32("r");

            var sub = new BinaryExpression(Operator.ISub, PrimitiveType.Word32, new MemoryAccess(MemoryIdentifier.GlobalMemory, r, PrimitiveType.Word32), Constant.Word32(0));
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
			var exp = sub.Accept(vp);
			Assert.AreEqual("Mem0[r:word32]", exp.ToString());
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpEquality2()
		{
            // Makes sure that 
            // y = x - 2
            // if (y == 0) ...
            // doesn't get munged into
            // y = x - 2
            // if (x == 2)

            ProcedureBuilder m = new ProcedureBuilder();
			Identifier x = m.Reg32("x", 0);
			Identifier y = m.Reg32("y", 1);
            m.Assign(x, m.LoadDw(Constant.Word32(0x1000300)));
            m.Assign(y, m.ISub(x, 2));
			m.BranchIf(m.Eq(y, 0), "test");
            m.Return();
            m.Label("test");
            m.Return();
            var importResolver = mr.Stub<IImportResolver>();
            importResolver.Replay();
            var sst = new SsaTransform(
                program, 
                m.Procedure, 
                new HashSet<Procedure>(),
                importResolver, 
                new ProgramDataFlow());
            sst.Transform();

            var vp = new ValuePropagator(arch, sst.SsaState, listener);
            var stm = m.Procedure.EntryBlock.Succ[0].Statements.Last;
			vp.Transform(stm);
			Assert.AreEqual("branch x_2 == 0x00000002 test", stm.Instruction.ToString());
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpCopyPropagate()
		{
            var m = new ProcedureBuilder();
			Identifier x = m.Reg32("x", 0);
			Identifier y = m.Reg32("y", 1);
			Identifier z = m.Reg32("z", 2);
			Identifier w = m.Reg32("w", 3);
            m.Assign(x, m.LoadDw(m.Word32(0x10004000)));
            m.Assign(y, x);
            m.Assign(z, m.IAdd(y, 2));
            m.Assign(w, y);

            var importResolver = mr.Stub<IImportResolver>();
            importResolver.Replay();

            var sst = new SsaTransform(
                program,
                m.Procedure, 
                new HashSet<Procedure>(),
                importResolver, 
                new ProgramDataFlow());
            sst.Transform();
            ssaIds = sst.SsaState.Identifiers;
            var stms = m.Procedure.EntryBlock.Succ[0].Statements;
			Assert.AreEqual("x_2 = Mem0[0x10004000:word32]", stms[0].ToString());
			Assert.AreEqual("y_3 = x_2", stms[1].ToString());
			Assert.AreEqual("z_4 = y_3 + 0x00000002", stms[2].ToString());
			Assert.AreEqual("w_5 = y_3", stms[3].ToString());
            //Debug.Print("{0}", string.Join(", ", ssaIds.Single(i => i.Identifier.Name == "x_1").Uses));
            //Debug.Print("{0}", string.Join(", ", ssaIds.Single(i => i.Identifier.Name == "y_2").Uses));
            //Debug.Print("{0}", string.Join(", ", ssaIds.Single(i => i.Identifier.Name == "z_3").Uses));
            //Debug.Print("{0}", string.Join(", ", ssaIds.Single(i => i.Identifier.Name == "w_4").Uses));


            ValuePropagator vp = new ValuePropagator(arch, sst.SsaState, listener);
            stms.ForEach(s => vp.Transform(s));

			Assert.AreEqual("x_2 = Mem0[0x10004000:word32]", stms[0].ToString());
			Assert.AreEqual("y_3 = x_2", stms[1].ToString());
			Assert.AreEqual("z_4 = x_2 + 0x00000002", stms[2].ToString());
			Assert.AreEqual("w_5 = x_2", stms[3].ToString());

            Assert.AreEqual(3, ssaIds.Single(i => i.Identifier.Name == "x_2").Uses.Count);
			Assert.AreEqual(0, ssaIds.Single(i => i.Identifier.Name == "y_3").Uses.Count);
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpSliceConstant()
		{
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
            Expression c = new Slice(PrimitiveType.Byte, Constant.Word32(0x10FF), 0).Accept(vp);
			Assert.AreEqual("0xFF", c.ToString());
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpNegSub()
		{
			Identifier x = Reg32("x");
			Identifier y = Reg32("y");
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
			Expression e = vp.VisitUnaryExpression(
				new UnaryExpression(Operator.Neg, PrimitiveType.Word32, new BinaryExpression(
				Operator.ISub, PrimitiveType.Word32, x, y)));
			Assert.AreEqual("y - x", e.ToString());
		}

		/// <summary>
		/// (<< (+ (* id c1) id) c2))
		/// </summary>
		[Test] 
        [Category(Categories.UnitTests)]
		public void VpMulAddShift()
		{
			Identifier id = Reg32("id");
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
			PrimitiveType t = PrimitiveType.Int32;
			BinaryExpression b = new BinaryExpression(Operator.Shl, t, 
				new BinaryExpression(Operator.IAdd, t, 
					new BinaryExpression(Operator.SMul, t, id, Constant.Create(t, 4)),
					id),
				Constant.Create(t, 2));
			Expression e = vp.VisitBinaryExpression(b);
			Assert.AreEqual("id *s 20", e.ToString());
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpShiftShift()
		{
			Identifier id = Reg32("id");
			ProcedureBuilder m = new ProcedureBuilder();
			Expression e = m.Shl(m.Shl(id, 1), 4);
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
			e = e.Accept(vp);
			Assert.AreEqual("id << 0x05", e.ToString());
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpShiftSum()
		{
			ProcedureBuilder m = new ProcedureBuilder();
			Expression e = m.Shl(1, m.ISub(Constant.Byte(32), 1));
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
			e = e.Accept(vp);
			Assert.AreEqual("0x80000000", e.ToString());
		}

		[Test]
        [Category(Categories.UnitTests)]
		public void VpSequenceOfConstants()
		{
			Constant pre = Constant.Word16(0x0001);
			Constant fix = Constant.Word16(0x0002);
			Expression e = new MkSequence(PrimitiveType.Word32, pre, fix);
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
			e = e.Accept(vp);
			Assert.AreEqual("0x00010002", e.ToString());
		}

        [Test]
        [Category(Categories.UnitTests)]
        public void SliceShift()
        {
            Constant eight = Constant.Word16(8);
            Identifier C = Reg8("C");
            Expression e = new Slice(PrimitiveType.Byte, new BinaryExpression(Operator.Shl, PrimitiveType.Word16, C, eight), 8);
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
            e = e.Accept(vp);
            Assert.AreEqual("C", e.ToString());
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void VpMkSequenceToAddress()
        {
            Constant seg = Constant.Create(PrimitiveType.SegmentSelector, 0x4711);
            Constant off = Constant.Word16(0x4111);
            arch.Expect(a => a.MakeSegmentedAddress(seg, off)).Return(Address.SegPtr(0x4711, 0x4111));
            mr.ReplayAll();

            Expression e = new MkSequence(PrimitiveType.Word32, seg, off);
            var vp = new ExpressionSimplifier(new SsaEvaluationContext(arch, ssaIds), listener);
            e = e.Accept(vp);
            Assert.IsInstanceOf(typeof(Address), e);
            Assert.AreEqual("4711:4111", e.ToString());

            mr.VerifyAll();
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void VpPhiWithConstants()
        {
            Constant c1 = Constant.Word16(0x4711);
            Constant c2 = Constant.Word16(0x4711);
            Identifier r1 = Reg16("r1");
            Identifier r2 = Reg16("r2");
            Identifier r3 = Reg16("r3");
            var stm1 = new Statement(1, new Assignment(r1, c1), null);
            var stm2 = new Statement(2, new Assignment(r2, c2), null);
            var proc = new Procedure("foo", arch.CreateFrame());
            var ssa = new SsaState(proc, null);
            var r1Sid = ssa.Identifiers.Add(r1, null, null, false);
            var r2Sid = ssa.Identifiers.Add(r2, null, null, false);
            r1Sid.DefStatement = stm1;
            r2Sid.DefStatement = stm2;
            var vp = new ValuePropagator(arch, ssa, listener);
            Instruction instr = new PhiAssignment(r3, new PhiFunction(r1.DataType, r1, r2));
            instr = instr.Accept(vp);
            Assert.AreEqual("r3 = 0x4711", instr.ToString());
        }

        [Test(Description =
            "if x = phi(a_1, a_2, ... a_n) and all phi arguments after " +
            "value propagation are equal to <exp> or x where <exp> is some  " +
            "expression then replace phi assignment with x = <exp>)")]
        [Category(Categories.UnitTests)]
        public void VpPhiLoops()
        {
            var m = new ProcedureBuilder();
            var ssa = new SsaState(m.Procedure, null);
            ssaIds = ssa.Identifiers;
            var fp = Reg16("fp");
            var a = Reg16("a");
            var b = Reg16("b");
            var c = Reg16("c");
            var d = Reg16("d");
            var x = Reg16("x");
            var y = Reg16("y");
            var z = Reg16("z");
            m.Emit(m.Assign(y, m.IAdd(x, 4)));
            m.Emit(m.Assign(z, m.ISub(x, 8)));
            m.Emit(m.Assign(a, m.ISub(fp, 12)));
            m.Emit(m.Assign(b, m.ISub(fp, 12)));
            m.Emit(m.Assign(c, m.ISub(y, 4)));
            m.Emit(m.Assign(d, m.IAdd(z, 8)));
            var phiStm = m.Phi(x, a, b, c, d);
            var stms = m.Procedure.EntryBlock.Succ[0].Statements;
            stms.ForEach(stm =>
            {
                var ass = stm.Instruction as Assignment;
                if (ass != null)
                    ssaIds[ass.Dst].DefStatement = stm;
                var phiAss = stm.Instruction as PhiAssignment;
                if (phiAss != null)
                    ssaIds[phiAss.Dst].DefStatement = stm;
            });
            var vp = new ValuePropagator(arch, ssa, listener);
            vp.Transform();
            Assert.AreEqual("x = fp - 0x000C", phiStm.Instruction.ToString());
        }

        [Test]
        [Category(Categories.IntegrationTests)]
        public void VpDbpDbp()
        {
            var m = new ProcedureBuilder();
            var d1 = m.Reg32("d32",0);
            var a1 = m.Reg32("a32",1);

            m.Assign(d1, m.Dpb(d1, m.LoadW(a1), 0));
            m.Assign(d1, m.Dpb(d1, m.LoadW(m.IAdd(a1, 4)), 0));

			Procedure proc = m.Procedure;
			var gr = proc.CreateBlockDominatorGraph();
            var importResolver = MockRepository.GenerateStub<IImportResolver>();
            importResolver.Replay();
			var sst = new SsaTransform(
                program,
                proc, 
                new HashSet<Procedure>(),
                importResolver,
                new ProgramDataFlow());
            sst.Transform();
			var ssa = sst.SsaState;

			var vp = new ValuePropagator(arch, ssa, listener);
			vp.Transform();

			using (FileUnitTester fut = new FileUnitTester("Analysis/VpDpbDpb.txt"))
			{
				proc.Write(false, fut.TextWriter);
				fut.TextWriter.WriteLine();
				fut.AssertFilesEqual();
			}
		}

        [Test(Description = "Casting a DPB should result in the deposited bits.")]
        [Category(Categories.UnitTests)]
        public void VpLoadDpb()
        {
            var m = new ProcedureBuilder();
            var a2 = m.Reg32("a2", 10);
            var d3 = m.Reg32("d3", 3);
            var tmp = m.Temp(PrimitiveType.Byte, "tmp");

            m.Assign(tmp, m.LoadB(a2));
            m.Assign(d3, m.Dpb(d3, tmp, 0));
            m.Store(m.IAdd(a2, 4), m.Cast(PrimitiveType.Byte, d3));

            SsaState ssa = RunTest(m);

            var sExp =
            #region Expected
@"a2:a2
    def:  def a2
    uses: tmp_3 = Mem0[a2:byte]
          Mem6[a2 + 0x00000004:byte] = tmp_3
Mem0:Global
    def:  def Mem0
    uses: tmp_3 = Mem0[a2:byte]
tmp_3: orig: tmp
    def:  tmp_3 = Mem0[a2:byte]
    uses: d3_5 = DPB(d3, tmp_3, 0)
          Mem6[a2 + 0x00000004:byte] = tmp_3
d3:d3
    def:  def d3
    uses: d3_5 = DPB(d3, tmp_3, 0)
d3_5: orig: d3
    def:  d3_5 = DPB(d3, tmp_3, 0)
Mem6: orig: Mem0
    def:  Mem6[a2 + 0x00000004:byte] = tmp_3
// ProcedureBuilder
// Return size: 0
define ProcedureBuilder
ProcedureBuilder_entry:
	def a2
	def Mem0
	def d3
	// succ:  l1
l1:
	tmp_3 = Mem0[a2:byte]
	d3_5 = DPB(d3, tmp_3, 0)
	Mem6[a2 + 0x00000004:byte] = tmp_3
ProcedureBuilder_exit:
";
            #endregion

            AssertStringsEqual(sExp, ssa);
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void VpLoadDpbSmallerCast()
        {
            var m = new ProcedureBuilder();
            var a2 = m.Reg32("a2", 10);
            var d3 = m.Reg32("d3", 3);
            var tmp = m.Temp(PrimitiveType.Word16, "tmp");

            m.Assign(tmp, m.LoadW(a2));
            m.Assign(d3, m.Dpb(d3, tmp, 0));
            m.Store(m.IAdd(a2, 4), m.Cast(PrimitiveType.Byte, d3));

            SsaState ssa = RunTest(m);

            var sExp =
            #region Expected
@"a2:a2
    def:  def a2
    uses: tmp_3 = Mem0[a2:word16]
          Mem6[a2 + 0x00000004:byte] = (byte) tmp_3
Mem0:Global
    def:  def Mem0
    uses: tmp_3 = Mem0[a2:word16]
tmp_3: orig: tmp
    def:  tmp_3 = Mem0[a2:word16]
    uses: d3_5 = DPB(d3, tmp_3, 0)
          Mem6[a2 + 0x00000004:byte] = (byte) tmp_3
d3:d3
    def:  def d3
    uses: d3_5 = DPB(d3, tmp_3, 0)
d3_5: orig: d3
    def:  d3_5 = DPB(d3, tmp_3, 0)
Mem6: orig: Mem0
    def:  Mem6[a2 + 0x00000004:byte] = (byte) tmp_3
// ProcedureBuilder
// Return size: 0
define ProcedureBuilder
ProcedureBuilder_entry:
	def a2
	def Mem0
	def d3
	// succ:  l1
l1:
	tmp_3 = Mem0[a2:word16]
	d3_5 = DPB(d3, tmp_3, 0)
	Mem6[a2 + 0x00000004:byte] = (byte) tmp_3
ProcedureBuilder_exit:
";
            #endregion

            AssertStringsEqual(sExp, ssa);
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void VpCastRealConstant()
        {
            var m = new ProcedureBuilder();
            var r1 = m.Reg32("r1", 1);

            m.Assign(r1, m.Cast(PrimitiveType.Real32, ConstantReal.Real64(1)));

            var ssa = RunTest(m);
            var sExp =
            #region Expected
@"r1_1: orig: r1
    def:  r1_1 = 1.0F
// ProcedureBuilder
// Return size: 0
define ProcedureBuilder
ProcedureBuilder_entry:
	// succ:  l1
l1:
	r1_1 = 1.0F
ProcedureBuilder_exit:
";
            #endregion

            AssertStringsEqual(sExp, ssa);
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void VpUndoUnnecessarySlicingOfSegmentPointer()
        {
            var m = new ProcedureBuilder();
            var es = m.Reg16("es", 1);
            var bx = m.Reg16("bx", 3);
            var es_bx = m.Frame.EnsureSequence(es.Storage, bx.Storage, PrimitiveType.Word32);

            m.Assign(es_bx, m.SegMem(PrimitiveType.Word32, es, bx));
            m.Assign(es, m.Slice(PrimitiveType.Word16, es_bx, 16));
            m.Assign(bx, m.Cast(PrimitiveType.Word16, es_bx));
            m.SegStore(es, m.IAdd(bx, 4), m.Byte(3));

            var ssa = RunTest(m);

            var sExp =
            #region Expected
@"es:es
    def:  def es
    uses: es_bx_4 = Mem0[es:bx:word32]
bx:bx
    def:  def bx
    uses: es_bx_4 = Mem0[es:bx:word32]
Mem0:Global
    def:  def Mem0
    uses: es_bx_4 = Mem0[es:bx:word32]
es_bx_4: orig: es_bx
    def:  es_bx_4 = Mem0[es:bx:word32]
    uses: es_5 = SLICE(es_bx_4, word16, 16) (alias)
          bx_6 = (word16) es_bx_4 (alias)
          es_7 = SLICE(es_bx_4, word16, 16)
          bx_8 = (word16) es_bx_4
          Mem9[es_bx_4 + 0x0004:byte] = 0x03
es_5: orig: es
    def:  es_5 = SLICE(es_bx_4, word16, 16) (alias)
bx_6: orig: bx
    def:  bx_6 = (word16) es_bx_4 (alias)
es_7: orig: es
    def:  es_7 = SLICE(es_bx_4, word16, 16)
bx_8: orig: bx
    def:  bx_8 = (word16) es_bx_4
Mem9: orig: Mem0
    def:  Mem9[es_bx_4 + 0x0004:byte] = 0x03
// ProcedureBuilder
// Return size: 0
define ProcedureBuilder
ProcedureBuilder_entry:
	def es
	def bx
	def Mem0
	// succ:  l1
l1:
	es_bx_4 = Mem0[es:bx:word32]
	es_5 = SLICE(es_bx_4, word16, 16) (alias)
	bx_6 = (word16) es_bx_4 (alias)
	es_7 = SLICE(es_bx_4, word16, 16)
	bx_8 = (word16) es_bx_4
	Mem9[es_bx_4 + 0x0004:byte] = 0x03
ProcedureBuilder_exit:
";
            #endregion

            AssertStringsEqual(sExp, ssa);
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void VpMulBy6()
        {
            var m = new ProcedureBuilder();
            var r1 = m.Reg16("r1", 1);
            var r2 = m.Reg16("r1", 2);

            m.Assign(r2, r1);                 // r1
            m.Assign(r1, m.Shl(r1, 1));       // r1 * 2
            m.Assign(r1, m.IAdd(r1, r2));     // r1 * 3
            m.Assign(r1, m.Shl(r1, 1));       // r1 * 6

            var ssa = RunTest(m);
            var sExp =
            #region Expected
@"r1:r1
    def:  def r1
    uses: r1_2 = r1
          r1_3 = r1 << 0x01
          r1_4 = r1 * 0x0003
          r1_5 = r1 * 0x0006
r1_2: orig: r1
    def:  r1_2 = r1
r1_3: orig: r1
    def:  r1_3 = r1 << 0x01
r1_4: orig: r1
    def:  r1_4 = r1 * 0x0003
r1_5: orig: r1
    def:  r1_5 = r1 * 0x0006
// ProcedureBuilder
// Return size: 0
define ProcedureBuilder
ProcedureBuilder_entry:
	def r1
	// succ:  l1
l1:
	r1_2 = r1
	r1_3 = r1 << 0x01
	r1_4 = r1 * 0x0003
	r1_5 = r1 * 0x0006
ProcedureBuilder_exit:
";
            #endregion

            AssertStringsEqual(sExp, ssa);
        }

        [Test]
        [Category(Categories.UnitTests)]
        public void VpIndirectCall()
        {
            var callee = CreateExternalProcedure("foo", RegArg(1, "r1"), StackArg(4), StackArg(8));
            var pc = new ProcedureConstant(PrimitiveType.Pointer32, callee);

            var m = new ProcedureBuilder();
            var r1 = m.Reg32("r1", 1);
            var sp = m.Frame.EnsureRegister(m.Architecture.StackRegister);
            m.Assign(sp, m.Frame.FramePointer);
            m.Assign(r1, pc);
            m.Assign(sp, m.ISub(sp, 4));
            m.Store(sp, 3);
            m.Assign(sp, m.ISub(sp, 4));
            m.Store(sp, m.LoadW(m.Word32(0x1231230)));
            m.Call(r1, 4);
            m.Return();

            arch.Stub(a => a.CreateStackAccess(null, 0, null))
                .IgnoreArguments()
                .Do(new Func<IStorageBinder, int, DataType, Expression>((f, off, dt) => m.Load(dt, m.IAdd(f.EnsureIdentifier(sp.Storage), off))));
            arch.Stub(s => s.CreateFrameApplicationBuilder(null, null, null)).IgnoreArguments().Do(
                new Func<IStorageBinder, CallSite, Expression, FrameApplicationBuilder>(
                (frame, site, c) => new FrameApplicationBuilder(arch, frame, site, c, false)));

            mr.ReplayAll();

            var ssa = RunTest(m);
            var sExp =
            #region Expected
@"fp:fp
    def:  def fp
    uses: r63_2 = fp
          r63_4 = fp - 0x00000004
          Mem5[fp - 0x00000004:word32] = 0x00000003
          r63_6 = fp - 0x00000008
          Mem7[fp - 0x00000008:word16] = Mem5[0x01231230:word16]
          r1_8 = foo(Mem7[fp - 0x00000008:word32], Mem7[fp - 0x00000004:word32])
          r1_8 = foo(Mem7[fp - 0x00000008:word32], Mem7[fp - 0x00000004:word32])
r63_2: orig: r63
    def:  r63_2 = fp
r1_3: orig: r1
    def:  r1_3 = foo
r63_4: orig: r63
    def:  r63_4 = fp - 0x00000004
Mem5: orig: Mem0
    def:  Mem5[fp - 0x00000004:word32] = 0x00000003
    uses: Mem7[fp - 0x00000008:word16] = Mem5[0x01231230:word16]
r63_6: orig: r63
    def:  r63_6 = fp - 0x00000008
Mem7: orig: Mem0
    def:  Mem7[fp - 0x00000008:word16] = Mem5[0x01231230:word16]
    uses: r1_8 = foo(Mem7[fp - 0x00000008:word32], Mem7[fp - 0x00000004:word32])
          r1_8 = foo(Mem7[fp - 0x00000008:word32], Mem7[fp - 0x00000004:word32])
r1_8: orig: r1
    def:  r1_8 = foo(Mem7[fp - 0x00000008:word32], Mem7[fp - 0x00000004:word32])
// ProcedureBuilder
// Return size: 0
define ProcedureBuilder
ProcedureBuilder_entry:
	def fp
	// succ:  l1
l1:
	r63_2 = fp
	r1_3 = foo
	r63_4 = fp - 0x00000004
	Mem5[fp - 0x00000004:word32] = 0x00000003
	r63_6 = fp - 0x00000008
	Mem7[fp - 0x00000008:word16] = Mem5[0x01231230:word16]
	r1_8 = foo(Mem7[fp - 0x00000008:word32], Mem7[fp - 0x00000004:word32])
	return
	// succ:  ProcedureBuilder_exit
ProcedureBuilder_exit:
";
            #endregion
                AssertStringsEqual(sExp, ssa);
        }

        [Test]
        [Category(Categories.IntegrationTests)]
        public void VpCastCast()
        {
            var m = new ProcedureBuilder();
            m.Store(
                m.Word32(0x1234000),
                m.Cast(
                    PrimitiveType.Real32,
                    m.Cast(
                        PrimitiveType.Real64, 
                        m.Load(PrimitiveType.Real32, m.Word32(0x123400)))));
            m.Return();
            mr.ReplayAll();

            Assert.IsNotNull(importResolver);
            RunFileTest(m, "Analysis/VpCastCast.txt");
        }

        [Test(Description = "m68k floating-point comparison")]
        [Category(Categories.IntegrationTests)]
        public void VpFCmp()
        {
            var m = new FCmpFragment();
            mr.ReplayAll();

            RunFileTest(m, "Analysis/VpFCmp.txt");
        }

        [Test(Description = "Should be able to simplify address +/- constant")]
        [Category(Categories.IntegrationTests)]
        public void VpAddress32Const()
        {
            var m = new ProcedureBuilder("VpAddress32Const");
            var r1 = m.Reg32("r1", 1);
            m.Assign(r1, Address.Ptr32(0x00123400));
            m.Assign(r1, m.Load(r1.DataType, m.IAdd(r1, 0x56)));
            m.Return();

            mr.ReplayAll();
            RunFileTest(m, "Analysis/VpAddress32Const.txt");
        }
    }
}
