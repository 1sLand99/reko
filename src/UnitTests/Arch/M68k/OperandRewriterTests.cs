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

using NUnit.Framework;
using Reko.Arch.M68k;
using Reko.Arch.M68k.Machine;
using Reko.Arch.M68k.Rewriter;
using Reko.Core;
using Reko.Core.Expressions;
using Reko.Core.Rtl;
using Reko.Core.Types;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Reko.UnitTests.Arch.M68k
{
    [TestFixture]
    public class OperandRewriterTests
    {
        private M68kArchitecture arch;
        private Address addrInstr;

        [SetUp]
        public void Setup()
        {
            this.arch = new M68kArchitecture(new ServiceContainer(), "m68k", new Dictionary<string, object>());
            this.addrInstr = Address.Ptr32(0x0012340C);
        }

        [Test]
        public void M68korw_MemOp()
        {
            var orw = new OperandRewriter(
                new RtlEmitter(new List<RtlInstruction>()),
                arch.CreateFrame(),
                PrimitiveType.Byte);
            var exp = orw.RewriteSrc(new MemoryOperand(PrimitiveType.Byte, Registers.a1, Constant.Int16(4)), addrInstr);

            var ea = (BinaryExpression) ((MemoryAccess) exp).EffectiveAddress;
            Assert.AreEqual(ea.Left.DataType.Size, ea.Right.DataType.Size);
        }
    }
}
