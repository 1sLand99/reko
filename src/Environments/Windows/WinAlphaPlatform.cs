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
using Reko.Core.Hll.C;
using Reko.Core.Loading;
using Reko.Core.Machine;
using Reko.Core.Memory;
using Reko.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Reko.Environments.Windows
{
    public class WinAlphaPlatform : Platform
    {
        private AlphaCallingConvention cc;

        public WinAlphaPlatform(IServiceProvider services, IProcessorArchitecture arch) 
            : base(services, arch, "winAlpha")
        {
            arch.StackRegister = arch.GetRegister("r30")!;
            cc = null!;
            this.StructureMemberAlignment = 8;
            this.TrashedRegisters = CreateTrashedRegisters();
        }

        public override string DefaultCallingConvention => "";

        public override CParser CreateCParser(TextReader rdr, ParserState? state)
        {
            state ??= new ParserState();
            var lexer = new CLexer(rdr, CLexer.MsvcKeywords);
            var parser = new CParser(state, lexer);
            return parser;
        }

        private HashSet<RegisterStorage> CreateTrashedRegisters()
        {
            return new HashSet<RegisterStorage>(new[] {
                "r0", "r1", "r2", "r3", "r4", "r5", "r6", "r7", "r8",
                "r16", "r17", "r18", "r9", "r20", "r21",
                "r22", "r23", "r24", "r25", "r26", "r27", "r28"}.Select(n => Architecture.GetRegister(n)!).ToHashSet());
        }

        public override ImageSymbol? FindMainProcedure(Program program, Address addrStart)
        {
            Services.RequireService<IEventListener>().Warn(new NullCodeLocation(program.Name),
                           "Win32 Alpha main procedure finder not supported.");
            return null;
        }

        public override SystemService FindService(int vector, ProcessorState? state, IMemory? memory)
        {
            throw new NotImplementedException();
        }

        public override int GetBitSizeFromCBasicType(CBasicType cb)
        {
            throw new NotImplementedException();
        }

        public override ICallingConvention GetCallingConvention(string? ccName)
        {
            if (this.cc is null)
            {
                this.cc = new AlphaCallingConvention(this.Architecture);
            }
            return cc;
        }

        public override ExternalProcedure? LookupProcedureByName(string? moduleName, string procName)
        {
            return null;
        }
    }
}
