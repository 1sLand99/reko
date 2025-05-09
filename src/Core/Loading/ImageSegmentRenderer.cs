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

using Reko.Core.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reko.Core.Loading
{
    /// <summary>
    /// Base class for optional renderers for image segments. It is used to 
    /// render the contents of a loaded image segment differently from just 
    /// dumping its contents as a bunch of bytes.
    /// </summary>
    public abstract class ImageSegmentRenderer
    {
        /// <summary>
        /// Renders the contents of the image segment.
        /// </summary>
        /// <param name="segment">Segment to render.</param>
        /// <param name="program">Program context of the segment.</param>
        /// <param name="formatter">Output sink.</param>
        public abstract void Render(ImageSegment segment, Program program, Formatter formatter);
    }
}
