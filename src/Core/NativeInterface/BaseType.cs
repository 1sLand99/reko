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

namespace Reko.Core.NativeInterface
{
    /// <summary>
    /// Enumeration of all base types that can be used in native code,
    /// each corresponding to a static member of <see cref="Types.PrimitiveType"/>.
    /// </summary>
    [NativeInterop]
    public enum BaseType
    {
#pragma warning disable CS1591
        Void,

        Bool,

        Byte,
        SByte,
        Char8,

        Int16,
        UInt16,
        Ptr16,
        Word16,

        Int32,
        UInt32,
        Ptr32,
        Word32,

        Int64,
        UInt64,
        Ptr64,
        Word64,

        Word128,

        Real32,
        Real64,
    }
}