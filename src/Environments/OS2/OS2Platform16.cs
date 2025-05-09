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
using System;
using System.Collections.Generic;
using Reko.Arch.X86;
using System.Linq;
using System.IO;
using Reko.Core.Services;
using System.Text;
using Reko.Core.Types;
using Reko.Core.Machine;
using Reko.Core.Memory;

namespace Reko.Environments.OS2
{
    /// <summary>
    /// <see cref="Platform"/> class representing 16-bit OS/2 versions 1.0, 1.2, and 1.3
    /// </summary>
    /// <remarks>
    /// There will be a lot of similarities with Win16 here.
    /// http://www.edm2.com/index.php/OS/2_DLL_Entry_Points
    /// </remarks>
    public class OS2Platform16 : Platform
    {
        private static readonly SystemService int3svc = new SystemService
        {
            Name = "DebugBreak",
            Signature = FunctionType.Action(),
            Characteristics = Core.Serialization.DefaultProcedureCharacteristics.Instance,
            SyscallInfo = new SyscallInfo
            {
                 Vector = 3,
            }
        };

        private static readonly HashSet<RegisterStorage> implicitRegs = new HashSet<RegisterStorage>
        {
            Registers.cs,
            Registers.ss,
            Registers.sp,
            Registers.Top,
        };

        public OS2Platform16(IServiceProvider services, IProcessorArchitecture arch) : base(services, arch, "os2-16")
        {
            this.StructureMemberAlignment = 4;
            this.TrashedRegisters = CreateTrashedRegisters();
        }

        public override string DefaultCallingConvention => "pascal";

        public override CParser CreateCParser(TextReader rdr, ParserState? state)
        {
            state ??= new ParserState();
            var lexer = new CLexer(rdr, CLexer.MsvcKeywords);
            var parser = new CParser(state, lexer);
            return parser;
        }

        public override bool IsImplicitArgumentRegister(RegisterStorage reg)
        {
            return implicitRegs.Contains(reg);
        }

        private HashSet<RegisterStorage> CreateTrashedRegisters()
        {
            // Some calling conventions can save registers, like _watcall
            return new HashSet<RegisterStorage>
            {
                Registers.ax,
                Registers.bx,
                Registers.cx,
                Registers.dx,
                Registers.es,
                Registers.Top,
            };
        }

        public override SystemService? FindService(int vector, ProcessorState? state, IMemory? memory)
        {
            if (vector == 3)
            {
                return int3svc;
            }
            return null;
        }

        public override int GetBitSizeFromCBasicType(CBasicType cb)
        {
            switch (cb)
            {
                case CBasicType.Bool: return 8;
                case CBasicType.Char: return 8;
                case CBasicType.Short: return 16;
                case CBasicType.Int: return 16;
                case CBasicType.Long: return 32;
                case CBasicType.Float: return 32;
                case CBasicType.Double: return 64;
                // Seen in Watcom
                case CBasicType.Int64: return 64;
                // Seen in OpenWatcom as an alias to __int64
                case CBasicType.LongLong: return 64;
                // Used for EBCDIC, Shift-JIS and Unicode
                case CBasicType.WChar_t: return 16;
            }
            throw new NotImplementedException();
        }

        public override ICallingConvention GetCallingConvention(string? ccName)
        {
            switch (ccName ?? "")
            {
            case "":
            // Used by Microsoft C
            case "__cdecl":
            case "cdecl":
                return new X86CallingConvention(2, 4, true, false);
            // Default for system libraries
            case "pascal":
            case "__pascal":
                return new X86CallingConvention(2, 4, false, true);
            }
            throw new NotSupportedException(string.Format("Calling convention '{0}' is not supported.", ccName));
        }

        public override ExternalProcedure? LookupProcedureByOrdinal(string? moduleName, int ordinal)
        {
            var metadata = EnsureTypeLibraries(PlatformIdentifier);
            foreach (var tl in metadata.Modules.Values.Where(t => string.Compare(t.ModuleName, moduleName, true) == 0))
            {
                if (tl.ServicesByOrdinal.TryGetValue(ordinal, out SystemService? svc))
                {
                    // Found the name of the imported procedure, now try to find its signature.
                    string procName = svc.Name!;
                    if (!metadata.Signatures.TryGetValue(procName, out var sig))
                    {
                        return new ExternalProcedure(procName, svc.Signature!);
                    }
                    else
                    {
                        var chr = LookupCharacteristicsByName(procName);
                        return new ExternalProcedure(procName, sig, chr);
                    }
                }
            }
            return null;
        }

        public override ExternalProcedure? LookupProcedureByName(string? moduleName, string procName)
        {
            var metadata = EnsureTypeLibraries(PlatformIdentifier);
            if (moduleName is not null && metadata.Modules.TryGetValue(moduleName.ToUpper(), out ModuleDescriptor? mod))
            {
                if (mod.ServicesByName.TryGetValue(procName, out SystemService? svc))
                {
                    var chr = LookupCharacteristicsByName(svc.Name!);
                    return new ExternalProcedure(svc.Name!, svc.Signature!, chr);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                if (!metadata.Signatures.TryGetValue(procName, out var sig))
                    return null;
                var chr = LookupCharacteristicsByName(procName);
                return new ExternalProcedure(procName, sig, chr);
            }
        }

        public override void WriteMetadata(Program program, string path)
        {
            WriteModuleDefinition(program, Path.ChangeExtension(path, ".def"));
        }

        public void WriteModuleDefinition(Program program, string path)
        {
            var fsSvc = Services.RequireService<IFileSystemService>();
            var dir = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(dir))
                fsSvc.CreateDirectory(dir);
            using (var w = fsSvc.CreateStreamWriter(path, false, new UTF8Encoding(false)))
            {
                var filename = program.Location.GetFilename();
                w.WriteLine("; {0}", Path.GetFileName(path));
                w.WriteLine("; Generated by decompiling {0}", filename);
                w.WriteLine("; using Reko decompiler version {0}.", AssemblyMetadata.AssemblyFileVersion);
                w.WriteLine();
                w.WriteLine("LIBRARY {0}", filename);
                w.WriteLine("EXPORTS");
                foreach (var entryPoint in program.EntryPoints.Values.OrderBy(e => e.Ordinal))
                {
                    if (entryPoint.Ordinal.HasValue)
                    {
                        w.WriteLine("{0} @{1}", entryPoint.Name, entryPoint.Ordinal.Value);
                    }
                    else
                    {
                        w.WriteLine("{0}", entryPoint.Name);
                    }
                }
            }
        }
    }
}
