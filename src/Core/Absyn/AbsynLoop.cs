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

using Reko.Core.Expressions;
using System;
using System.Collections.Generic;

namespace Reko.Core.Absyn
{
	/// <summary>
	/// Abstract base class for Representation of loops.
	/// </summary>
	public abstract class AbsynLoop : AbsynStatement
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="AbsynLoop"/> class.
        /// </summary>
        /// <param name="condition">The predicate controlling the loop.</param>
        /// <param name="body">The body of the loop.</param>
		protected AbsynLoop(Expression condition, List<AbsynStatement> body)
		{
            if (condition is null)
                throw new ArgumentNullException(nameof(condition));
			this.Condition = condition;
			this.Body = body;
		}

        /// <summary>
        /// The body of the loop.
        /// </summary>
		public List<AbsynStatement> Body { get; }

        /// <summary>
        /// The predicate controlling the loop.
        /// </summary>
		public Expression Condition { get; }
	}
}
