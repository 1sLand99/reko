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

using Reko.Core;
using Reko.Core.Machine;
using Reko.Core.Types;

namespace Reko.Arch.Sanyo
{
    public class MemoryOperand : AbstractMachineOperand
    {
        public MemoryOperand(PrimitiveType dt, RegisterStorage @base, bool indirect)
            : base(dt)
        {
            this.Base = @base;
            this.Indirect = indirect;
        }

        public MemoryOperand(PrimitiveType dt, ushort offset)
            : base(dt)
        {
            this.Offset = offset;
        }

        public RegisterStorage? Base { get; }
        public bool Indirect { get; }
        public ushort Offset { get; }

        protected override void DoRender(MachineInstructionRenderer renderer, MachineInstructionRendererOptions options)
        {
            if (Base is null)
            {
                renderer.WriteAddress($"${Offset:X4}", Address.Ptr16(Offset));
            }
            else
            {
                renderer.WriteChar('@');
                renderer.WriteString(Base!.Name);
            }
        }
    }
}