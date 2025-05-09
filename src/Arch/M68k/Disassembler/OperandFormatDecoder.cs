#region License
/* 
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

using Reko.Arch.M68k.Machine;
using Reko.Core;
using Reko.Core.Expressions;
using Reko.Core.Machine;
using Reko.Core.Memory;
using Reko.Core.Types;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reko.Arch.M68k.Disassembler
{
    /// <summary>
    /// Decodes M86k operands using a simple format language.
    /// </summary>
    public class OperandFormatDecoder
    {
        private readonly M68kDisassembler dasm;

        public OperandFormatDecoder(M68kDisassembler dasm, int i)
        {
            this.dasm = dasm;
        }

        private static PrimitiveType SizeField(ushort opcode, int bitOffset)
        {
            switch (opcode >> bitOffset & 3)
            {
            case 0: return PrimitiveType.Byte;
            case 1: return PrimitiveType.Word16;
            case 2: return PrimitiveType.Word32;
            default: throw new InvalidOperationException(string.Format("Illegal size field in opcode {0:X4}.", opcode));
            }
        }

        public bool TryParseOperand(ushort opcode, int bitOffset, PrimitiveType dataWidth, EndianImageReader rdr, out MachineOperand op)
        {
            opcode >>= bitOffset;
            byte operandBits = (byte) (opcode & 7);
            byte addressMode = (byte) (opcode >> 3 & 7);
            return TryParseOperandInner(opcode, addressMode, operandBits, dataWidth, rdr, out op);
        }

        private bool TryParseOperandInner(ushort uInstr, byte addressMode, byte operandBits, PrimitiveType dataWidth, EndianImageReader rdr, out MachineOperand op)
        {
            Constant? offset;
            switch (addressMode)
            {
            case 0: // Data register direct.
                op = DataRegisterOperand(operandBits, 0);
                return true;
            case 1: // Address register direct
                op = AddressRegister(operandBits, 0);
                return true;
            case 2:  // Address register indirect
                op = MemoryOperand.Indirect(dataWidth, AddressRegister(operandBits, 0));
                return true;
            case 3:  // Address register indirect with postincrement.
                op = MemoryOperand.PostIncrement(dataWidth, AddressRegister(operandBits, 0));
                return true;
            case 4:  // Address register indirect with predecrement.
                op = MemoryOperand.PreDecrement(dataWidth, AddressRegister(operandBits, 0));
                return true;
            case 5: // Address register indirect with displacement.
                if (!rdr.TryReadBe(PrimitiveType.Int16, out offset))
                {
                    op = default!;
                    return false;
                }
                op = MemoryOperand.Indirect(dataWidth, AddressRegister(operandBits, 0), offset);
                return true;
            case 6: // Address register indirect with index
                return TryAddressRegisterIndirectWithIndex(uInstr, dataWidth, rdr, out op);
            case 7:
                switch (operandBits)
                {
                case 0: // Absolute short address
                    ushort usAddr;
                    if (!rdr.TryReadBeUInt16(out usAddr))
                    {
                        op = default!; return false;
                    }
                    op = Address.Ptr32(usAddr);
                    return true;
                case 1: // Absolute long address
                    uint uAddr;
                    if (!rdr.TryReadBeUInt32(out uAddr))
                    {
                        op = default!; return false;
                    }
                    op = Address.Ptr32(uAddr);
                    return true;
                case 2: // Program counter with displacement
                    var off = rdr.Address - dasm.addr;
                    short sOffset;
                    if (!rdr.TryReadBeInt16(out sOffset))
                    {
                        op = default!; return false;
                    }
                    off += sOffset;
                    op = new MemoryOperand(dataWidth, Registers.pc, Constant.Int16((short) off));
                    return true;
                case 3:
                    // Program counter with index
                    var addrExt = rdr.Address;
                    ushort extension;
                    if (!rdr.TryReadBeUInt16(out extension))
                    {
                        op = default!; return false;
                    }

                    if (EXT_FULL(extension))
                    {
                        if (EXT_EFFECTIVE_ZERO(extension))
                        {
                            op = Constant.Word32(0);
                            return true;
                        }
                        Constant? @base = null;
                        Constant? outer = null;
                        if (EXT_BASE_DISPLACEMENT_PRESENT(extension))
                        {
                            if (!rdr.TryReadBe(
                                EXT_BASE_DISPLACEMENT_LONG(extension)
                                    ? PrimitiveType.Word32
                                    : PrimitiveType.Int16,
                                out @base))
                            {
                                op = default!; return false;
                            }
                        }
                        if (EXT_OUTER_DISPLACEMENT_PRESENT(extension))
                        {
                            if (!rdr.TryReadBe(
                                EXT_OUTER_DISPLACEMENT_LONG(extension)
                                    ? PrimitiveType.Word32
                                    : PrimitiveType.Int16,
                                out outer))
                            {
                                op = default!; return false;
                            }
                        }
                        RegisterStorage? base_reg = EXT_BASE_REGISTER_PRESENT(extension)
                            ? Registers.pc
                            : null;
                        RegisterStorage? index_reg = null;
                        PrimitiveType? index_width = null;
                        int index_scale = 0;
                        if (EXT_INDEX_REGISTER_PRESENT(extension))
                        {
                            index_reg = EXT_INDEX_AR(extension)
                                ? Registers.AddressRegister((int) EXT_INDEX_REGISTER(extension))
                                : Registers.DataRegister((int) EXT_INDEX_REGISTER(extension));
                            index_width = EXT_INDEX_LONG(extension) ? PrimitiveType.Word32 : PrimitiveType.Int16;
                            index_scale = EXT_INDEX_SCALE(extension) != 0
                                ? 1 << EXT_INDEX_SCALE(extension)
                                : 0;
                        }
                        op = new IndexedOperand(dataWidth, @base, outer, base_reg, index_reg, index_width, index_scale,
                            (extension & 7) > 0 && (extension & 7) < 4,
                            (extension & 7) > 4);
                        return true;
                    }
                    op = new IndirectIndexedOperand(
                        dataWidth,
                        EXT_8BIT_DISPLACEMENT(extension),
                        Registers.pc,
                        EXT_INDEX_AR(extension)
                            ? Registers.AddressRegister((int) EXT_INDEX_REGISTER(extension))
                            : Registers.DataRegister((int) EXT_INDEX_REGISTER(extension)),
                        EXT_INDEX_LONG(extension)
                            ? PrimitiveType.Word32
                            : PrimitiveType.Int16,
                        1 << EXT_INDEX_SCALE(extension));
                    return true;
                case 4:
                    //  Immediate
                    if (dataWidth.Size == 1)        // don't want the instruction stream to get misaligned!
                    {
                        rdr.Offset += 1;
                    }
                    if (!rdr.TryReadBe(dataWidth, out Constant? coff))
                    {
                        op = default!; return false;
                    }
                    op = coff;
                    return true;
                default:
                    op = default!;
                    return false;
                }
            default:
                throw new NotImplementedException(string.Format("Address mode {0:X1} not implemented.", addressMode));
            }
        }

        private bool TryAddressRegisterIndirectWithIndex(ushort uInstr, PrimitiveType dataWidth, EndianImageReader rdr, out MachineOperand op)
        {
            if (!rdr.TryReadBeUInt16(out ushort extension))
            {
                op = default!; return false;
            }
            if (EXT_FULL(extension))
            {
                if (M68kDisassembler.EXT_EFFECTIVE_ZERO(extension))
                {
                    op = Address.Ptr32(0);
                    return true;
                }

                RegisterStorage? base_reg = null;
                RegisterStorage? index_reg = null;
                PrimitiveType? index_reg_width = null;
                int index_scale = 1;
                Constant? @base = null;
                if (EXT_BASE_DISPLACEMENT_PRESENT(extension))
                {
                    if (!rdr.TryReadBe(
                        EXT_BASE_DISPLACEMENT_LONG(extension) ? PrimitiveType.Word32 : PrimitiveType.Int16,
                        out @base))
                    {
                        op = default!; return true;
                    }
                }

                Constant? outer = null;
                if (EXT_OUTER_DISPLACEMENT_PRESENT(extension))
                {
                    if (!rdr.TryReadBe(
                        EXT_OUTER_DISPLACEMENT_LONG(extension) ? PrimitiveType.Word32 : PrimitiveType.Int16,
                        out outer))
                    {
                        op = default!; return true;
                    }
                }
                if (EXT_BASE_REGISTER_PRESENT(extension))
                {
                    base_reg = Registers.AddressRegister(uInstr & 7);
                }
                if (EXT_INDEX_REGISTER_PRESENT(extension))
                {
                    index_reg = EXT_INDEX_AR(extension)
                        ? Registers.AddressRegister((int) EXT_INDEX_REGISTER(extension))
                        : Registers.DataRegister((int) EXT_INDEX_REGISTER(extension));
                    index_reg_width = EXT_INDEX_LONG(extension) ? PrimitiveType.Word32 : PrimitiveType.Word16;
                    if (EXT_INDEX_SCALE(extension) != 0)
                        index_scale = 1 << EXT_INDEX_SCALE(extension);
                }
                bool preindex = (extension & 7) > 0 && (extension & 7) < 4;
                bool postindex = (extension & 7) > 4;
                op = new IndexedOperand(dataWidth, @base, outer, base_reg, index_reg, index_reg_width, index_scale, preindex, postindex);
            }
            else
            {
                op = new IndirectIndexedOperand(
                    dataWidth,
                    EXT_8BIT_DISPLACEMENT(extension),
                    Registers.AddressRegister(uInstr & 7),
                    EXT_INDEX_AR(extension)
                        ? Registers.AddressRegister((int) EXT_INDEX_REGISTER(extension))
                        : Registers.DataRegister((int) EXT_INDEX_REGISTER(extension)),
                    EXT_INDEX_LONG(extension) ? PrimitiveType.Word32 : PrimitiveType.Int16,
                    1 << EXT_INDEX_SCALE(extension));
            }
            return true;
        }

        // Extension word formats
        private static sbyte EXT_8BIT_DISPLACEMENT(uint A) { return (sbyte) (A & 0xff); }
        internal static bool EXT_FULL(uint A) { return M68kDisassembler.BIT_8(A); }
        internal static bool EXT_EFFECTIVE_ZERO(uint A) { return (A & 0xe4) == 0xc4 || (A & 0xe2) == 0xc0; }
        private static bool EXT_BASE_REGISTER_PRESENT(uint A) { return !M68kDisassembler.BIT_7(A); }
        private static bool EXT_INDEX_REGISTER_PRESENT(uint A) { return !M68kDisassembler.BIT_6(A); }
        private static uint EXT_INDEX_REGISTER(uint A) { return A >> 12 & 7; }
        private static bool EXT_INDEX_PRE_POST(uint A) { return EXT_INDEX_REGISTER_PRESENT(A) && (A & 3) != 0; }
        private static bool EXT_INDEX_PRE(uint A) { return EXT_INDEX_REGISTER_PRESENT(A) && (A & 7) < 4 && (A & 7) != 0; }
        private static bool EXT_INDEX_POST(uint A) { return EXT_INDEX_REGISTER_PRESENT(A) && (A & 7) > 4; }
        internal static int EXT_INDEX_SCALE(uint A) { return (int) (A >> 9 & 3); }
        private static bool EXT_INDEX_LONG(uint A) { return M68kDisassembler.BIT_B(A); }
        private static bool EXT_INDEX_AR(uint A) { return M68kDisassembler.BIT_F(A); }
        private static bool EXT_BASE_DISPLACEMENT_PRESENT(uint A) { return (A & 0x30) > 0x10; }
        private static bool EXT_BASE_DISPLACEMENT_WORD(uint A) { return (A & 0x30) == 0x20; }
        private static bool EXT_BASE_DISPLACEMENT_LONG(uint A) { return (A & 0x30) == 0x30; }
        private static bool EXT_OUTER_DISPLACEMENT_PRESENT(uint A) { return (A & 3) > 1 && (A & 0x47) < 0x44; }
        private static bool EXT_OUTER_DISPLACEMENT_WORD(uint A) { return (A & 3) == 2 && (A & 0x47) < 0x44; }
        private static bool EXT_OUTER_DISPLACEMENT_LONG(uint A) { return (A & 3) == 3 && (A & 0x47) < 0x44; }

        private static RegisterStorage AddressRegister(ushort opcode, int bitOffset)
        {
            return Registers.GetRegister(8 + (opcode >> bitOffset & 0x7));
        }

        private static RegisterStorage DataRegisterOperand(ushort opcode, int bitOffset)
        {
            return Registers.GetRegister(opcode >> bitOffset & 0x7);
        }
    }
}
