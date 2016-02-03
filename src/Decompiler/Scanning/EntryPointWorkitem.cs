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
using System;
using System.Collections.Generic;
using System.Text;

namespace Reko.Scanning
{
    public class EntryPointWorkitem : WorkItem
    {
        private Scanner scanner;
        private Program program;
        private EntryPoint ep;

        public EntryPointWorkitem(Scanner scanner, Program program, EntryPoint ep)
        {
            this.scanner = scanner;
            this.program = program;
            this.ep = ep;
        }

        public override void Process()
        {
            scanner.ScanEntryPoint(program, ep);
        }
    }
}
