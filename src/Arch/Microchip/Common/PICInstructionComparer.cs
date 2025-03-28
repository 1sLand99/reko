#region License
/* 
 * Copyright (C) 2017-2025 Christian Hostelet.
 * inspired by work from:
 * Copyright (C) 1999-2025 John Källén.
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
using Reko.Core.Machine;
using System;

namespace Reko.Arch.MicrochipPIC.Common
{
    using Common;
    using Reko.Core.Expressions;

    /// <summary>
    /// Compares pairs of PIC instructions -- for equality only.
    /// </summary>
    /// <remarks>
    /// Used by the InstructionTrie class.
    /// </remarks>
    /// 
    public class PICInstructionComparer : InstructionComparer
    {
        public PICInstructionComparer(Normalize norm)
            : base(norm)
        {
        }

        public bool CompareOperands(MachineOperand opA, MachineOperand opB)
        {
            if (opA.GetType() != opB.GetType())
                return false;

            switch (opA)
            {
                case RegisterStorage regOpA:
                    var regOpB = (RegisterStorage)opB;
                    return NormalizeRegisters || regOpA == regOpB;

                case Constant immOpA:
                    var immOpB = (Constant)opB;
                    return NormalizeConstants || immOpA.Equals(immOpB);         // disregard immediate values.

                case Address addrOpA:
                    var addrOpB = (Address)opB;
                    return NormalizeConstants || addrOpA == addrOpB;

                case PICOperandPseudo pseudoA:
                    var pseudoB = (PICOperandPseudo)opB;
                    if (pseudoA.DataType != pseudoB.DataType)
                        return false;
                    if (pseudoA.Values.Length != pseudoB.Values.Length)
                        return false;
                    return NormalizeConstants || (pseudoA.Values == pseudoB.Values);

                case PICOperandImmediate immedOpA:
                    var immedOpB = (PICOperandImmediate)opB;
                    return NormalizeConstants || (immedOpA.ImmediateValue.Equals(immedOpB.ImmediateValue));

                case PICOperandFast fastOpA:
                    var fastOpB = (PICOperandFast)opB;
                    return NormalizeConstants || (fastOpA.IsFast == fastOpB.IsFast);

                case PICOperandRegister picregOpA:
                    var picregOpB = (PICOperandRegister)opB;
                    return NormalizeRegisters || picregOpA.Register == picregOpB.Register;

                case PICOperandProgMemoryAddress progMemOpA:
                    var progMemOpB = (PICOperandProgMemoryAddress)opB;
                    return NormalizeConstants || progMemOpA.CodeTarget == progMemOpB.CodeTarget;

                case PICOperandDataMemoryAddress dataMemOpA:
                    var dataMemOpB = (PICOperandDataMemoryAddress)opB;
                    return NormalizeConstants || dataMemOpA.DataTarget == dataMemOpB.DataTarget;

                case PICOperandBankedMemory bankmemOpA:
                    var bankmemOpB = (PICOperandBankedMemory)opB;
                    return NormalizeConstants || bankmemOpA.Offset == bankmemOpB.Offset;

                case PICOperandMemBitNo bitnoOpA:
                    var bitnoOpB = (PICOperandMemBitNo)opB;
                    return NormalizeConstants || bitnoOpA.BitNo == bitnoOpB.BitNo;

                case PICOperandMemWRegDest memWOpA:
                    var memWOpB = (PICOperandMemWRegDest)opB;
                    return NormalizeConstants || memWOpA.WRegIsDest == memWOpB.WRegIsDest;

                case PICOperandFSRNum fsrnumOpA:
                    var fsrnumOpB = (PICOperandFSRNum)opB;
                    return NormalizeRegisters || fsrnumOpA.FSRNum == fsrnumOpB.FSRNum;

                case PICOperandFSRIndexation fsridxOpA:
                    var fsridxOpB = (PICOperandFSRIndexation)opB;
                    return NormalizeConstants || (fsridxOpA.Offset == fsridxOpB.Offset && fsridxOpA.FSRNum == fsridxOpB.FSRNum);

                case PICOperandTBLRW tblOpA:
                    var tblOpB = (PICOperandTBLRW)opB;
                    return NormalizeConstants || tblOpA.TBLIncrMode == tblOpB.TBLIncrMode;

                case PICOperandTris trisOpA:
                    var trisOpB = (PICOperandTris)opB;
                    return NormalizeRegisters || trisOpA.TrisNum == trisOpB.TrisNum;

                default:
                    throw new NotImplementedException($"NYI: {opA.GetType()}");
            }
        }

        /// <summary>
        /// Implementation of IComparer.Compare. In reality, 
        /// </summary>
        /// <param name="oInstrA"></param>
        /// <param name="oInstrB"></param>
        /// <returns></returns>
        public override bool CompareOperands(MachineInstruction a, MachineInstruction b)
        {
            var instrA = (PICInstruction)a;
            var instrB = (PICInstruction)b;

            if (instrA.Mnemonic != instrB.Mnemonic)
                return false;
            if (instrA.NumberOfOperands != instrB.NumberOfOperands)
                return false;

            bool retval = true;
            if (instrA.NumberOfOperands > 0)
            {
                retval = CompareOperands(instrA.Operands[0], instrB.Operands[0]);
                if (retval && instrA.NumberOfOperands > 1)
                {
                    retval = CompareOperands(instrA.Operands[1], instrB.Operands[1]);
                    if (retval && instrA.NumberOfOperands > 2)
                    {
                        retval = CompareOperands(instrA.Operands[2], instrB.Operands[2]);
                    }
                }
            }
            return retval;
        }

        public override int GetOperandsHash(MachineInstruction inst)
        {
            var instr = (PICInstruction)inst;
            int hash = instr.NumberOfOperands.GetHashCode();
            if (instr.NumberOfOperands > 0)
            {
                hash = hash * 23 + GetHashCode(instr.Operands[0]);
                if (instr.NumberOfOperands > 1)
                {
                    hash = hash * 17 + GetHashCode(instr.Operands[1]);
                }
            }
            return hash;
        }

        private int GetHashCode(MachineOperand op)
        {
            switch (op)
            {
                case RegisterStorage regOp:
                    return GetRegisterHash(regOp);

                case Constant immOp:
                    return GetConstantHash(immOp);

                case Address addrOp:
                    return NormalizeConstants ? 1 : addrOp.GetHashCode();

                case PICOperandPseudo pseudoOp:
                    return NormalizeConstants ? 1 : pseudoOp.Values.GetHashCode();

                case PICOperandImmediate immedOp:
                    return GetConstantHash(immedOp.ImmediateValue);

                case PICOperandFast fastOp:
                    return fastOp.IsFast.GetHashCode();

                case PICOperandRegister picregOp:
                    return GetRegisterHash(picregOp.Register);

                case PICOperandProgMemoryAddress progMemOp:
                    return NormalizeConstants ? 1 : progMemOp.CodeTarget.GetHashCode();

                case PICOperandDataMemoryAddress dataMemOp:
                    return NormalizeConstants ? 1 : dataMemOp.DataTarget.GetHashCode();

                case PICOperandBankedMemory bankmemOp:
                    return NormalizeConstants ? 1 : bankmemOp.Offset.GetHashCode();

                case PICOperandMemBitNo bitnoOp:
                    return bitnoOp.BitNo.GetHashCode();

                case PICOperandMemWRegDest memWOp:
                    return memWOp.WRegIsDest.GetHashCode();

                case PICOperandFSRNum fsrnumOP:
                    return fsrnumOP.FSRNum.GetHashCode();

                case PICOperandFSRIndexation fsridxOp:
                    return (fsridxOp.FSRNum.GetHashCode() * 1217) ^ fsridxOp.Offset.GetHashCode();

                case PICOperandTBLRW tblOp:
                    return tblOp.TBLIncrMode.GetHashCode();

                case PICOperandTris trisOp:
                    return trisOp.TrisNum.GetHashCode();

                default:
                    throw new NotImplementedException("Unhandled operand type: " + op.GetType().FullName);
            }

        }

    }

}
