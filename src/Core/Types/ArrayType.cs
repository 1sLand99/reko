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

namespace Reko.Core.Types
{
    /// <summary>
    /// Represents an array of elements and its length. A length of 0 indicates the array may be arbitrarily long.
    /// </summary>
	public class ArrayType : DataType
	{
        /// <summary>
        /// Constructs an array type.
        /// </summary>
        /// <param name="elType">Array element type.</param>
        /// <param name="length">Number of elements in the array, or 0 if 
        /// the number of elements is unknown.
        /// </param>
		public ArrayType(DataType elType, int length) : base(Domain.Array)
		{
			this.ElementType = elType;
			this.Length = length;
		}

        /// <inheritdoc/>
        public override void Accept(IDataTypeVisitor v)
        {
            v.VisitArray(this);
        }

        /// <inheritdoc/>
        public override T Accept<T>(IDataTypeVisitor<T> v)
        {
            return v.VisitArray(this);
        }

        /// <inheritdoc/>
        public override DataType Clone(IDictionary<DataType, DataType>? clonedTypes)
		{
            return new ArrayType(ElementType, Length) { Qualifier = this.Qualifier };
		}

		/// <summary>
		/// DataType of each element in the array.
		/// </summary>
		public DataType ElementType { get; set; }

        /// <summary>
        /// True if the array is unbounded, i.e. has no known length.
        /// </summary>
        public bool IsUnbounded => Length == 0;

		/// <summary>
		/// Number of elements. 0 means unknown number of elements.
		/// </summary>
		public int Length { get; set; }

        /// <inheritdoc/>
		public override bool IsComplex => true;


        /// <inheritdoc/>
		public override int Size
		{
			get { return ElementType.Size * Length; }
			set { ThrowBadSize(); }
		}
	}
}
