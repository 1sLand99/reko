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

using System.Collections.Generic;

namespace Reko.Core.Configuration
{
    /// <summary>
    /// This class describes a particular model of a CPU processor architecture.
    /// </summary>
    public class ModelDefinition
    {
        /// <summary>
        /// The name of the CPU model.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// CPU options for that CPU model.
        /// </summary>
        public List<ListOption_v1> Options { get; set; } = new List<ListOption_v1>();
    }
}
