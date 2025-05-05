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

namespace Reko.Core.Serialization
{
    /// <summary>
    /// User-specified global variable.
    /// </summary>
    public class GlobalDataItem_v2
    {
        /// <summary>
        /// Address of the global variable.
        /// </summary>
        public string? Address;

        /// <summary>
        /// Data type of the global variable.
        /// </summary>
        public SerializedType? DataType;

        /// <summary>
        /// Name of the global variable.
        /// </summary>
        public string? Name;

        /// <summary>
        /// Optional user comment for the global variable.
        /// </summary>
        public string? Comment;
    }
}
