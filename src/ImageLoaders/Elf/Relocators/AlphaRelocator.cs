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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Reko.Core;
using Reko.Core.Loading;

namespace Reko.ImageLoaders.Elf.Relocators
{
    public class AlphaRelocator : ElfRelocator64
    {
        public AlphaRelocator(ElfLoader64 loader, SortedList<Address,ImageSymbol> imageSymbols) : base(loader, imageSymbols)
        {
        }

        public override (Address?, ElfSymbol?) RelocateEntry(RelocationContext ctx, ElfRelocation rela, ElfSymbol symbol)
        {
            return default;
        }

        public override string RelocationTypeToString(uint type)
        {
            //throw new NotImplementedException();
            return "";
        }
    }
}
