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
using Reko.Core.Configuration;
using Reko.Core.IO;
using Reko.Core.Loading;
using Reko.Core.Memory;
using Reko.Core.Services;
using System;
using System.Runtime.InteropServices;

namespace Reko.Environments.AtariTOS
{
    /// <summary>
    /// Loads Atari TOS PRG files.
    /// </summary>
    /// // https://github.com/yegord/snowman/issues/131
    public class PrgLoader : ProgramImageLoader
    {
        public PrgLoader(IServiceProvider services, ImageLocation imageUri, byte[] imgRaw) : base(services, imageUri, imgRaw)
        {
        }

        public override Address PreferredBaseAddress
        {
            get { return Address.Ptr32(0x00100000); }
            set { throw new NotImplementedException(); }
        }

        public override Program LoadProgram(Address? loadingAddr)
        {
            var rdr = new BeImageReader(RawImage);
            if (!TryLoadHeader(rdr, out var hdr))
                throw new BadImageFormatException();

            var addrLoad = loadingAddr ?? PreferredBaseAddress;
            var cfgSvc = Services.RequireService<IConfigurationService>();
            var arch = cfgSvc.GetArchitecture("m68k")!;
            var env = cfgSvc.GetEnvironment("atariTOS");
            var platform = env.Load(Services, arch);

            var bytes = new byte[hdr.TextSize + hdr.DataSize + hdr.BssSize];
            var mem = arch.CreateCodeMemoryArea(addrLoad, bytes);
            int cRead = rdr.ReadBytes(bytes, 0, hdr.TextSize + hdr.DataSize);
            if (cRead != hdr.TextSize + hdr.DataSize)
                throw new BadImageFormatException();

            var text = new ImageSegment(".text", addrLoad, mem, AccessMode.ReadExecute) { Size = hdr.TextSize };
            var data = new ImageSegment(".data", addrLoad + hdr.TextSize, mem, AccessMode.ReadWrite) { Size = hdr.DataSize };
            var bss = new ImageSegment(".bss", addrLoad + hdr.TextSize + hdr.DataSize, mem, AccessMode.ReadWrite) { Size = hdr.BssSize };
            //$TODO: Implement symbols. For now just skip over them.
            rdr.Offset += hdr.SymbolsSize;

            PerformRelocations(mem, rdr);

            var map = new SegmentMap(
                addrLoad,
                text, data, bss);
            var memory = new ByteProgramMemory(map);
            var program = new Program(memory, arch, platform);
            program.EntryPoints[addrLoad] = ImageSymbol.Location(program.Architecture, addrLoad);
            return program;
        }

        private bool TryLoadHeader(BeImageReader rdr, out PrgHeader hdr)
        {
            var h = rdr.ReadStruct<PrgHeader>();
            if (h.Magic != 0x601A)
            {
                hdr = default(PrgHeader);
                return false;
            }
            hdr = h;
            return true;
        }

        bool PerformRelocations(MemoryArea mem, ImageReader rdr)
        {
            if (!rdr.TryReadBeUInt32(out uint fixup))
                return false;
            if (fixup == 0)
                return true;    // no relocations to do.
            uint offset = fixup;
            for (;;)
            {
                var dst = mem.BaseAddress + offset;
                if (!mem.TryReadBeUInt32(offset, out uint l))
                    return false;
                l += mem.BaseAddress.ToUInt32();
                mem.WriteBeUInt32(offset, l);
                mem.Relocations.AddPointerReference(mem.BaseAddress.ToLinear() + offset, l);

                for(;;)
                {
                    if (!rdr.TryReadByte(out byte b))
                        return false;
                    if (b == 0)
                        return true;
                    else if (b == 1)
                    {
                        offset += 254;
                    }
                    else
                    {
                        offset += b;
                        break;
                    }
                }
            }
        }

        [Endian(Endianness.BigEndian)]
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct PrgHeader
        {
            public ushort Magic;
            public uint TextSize;
            public uint DataSize;
            public uint BssSize;
            public uint SymbolsSize;
            public uint Reserved1;
            public uint ProgramFlags;
            public ushort IsAbsolute;
        }
    }
}
