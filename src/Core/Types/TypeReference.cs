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
using System.IO;
using System.Text;

namespace Reko.Core.Types
{
    /// <summary>
    /// Refers to another type by name; think of C's <c>typedef</c>
    /// type builder.
    /// </summary>
    public class TypeReference : DataType
    {
        private DataType referent;

        /// <summary>
        /// Creates a type reference. The type reference refers to a <see cref="DataType"/>.
        /// </summary>
        /// <param name="dataType">The type being aliased.</param>
        public TypeReference(DataType dataType) : base(dataType.Domain)
        {
            this.referent = dataType;
        }

        /// <summary>
        /// Creates a type reference. The type reference refers to a <see cref="DataType"/>.
        /// </summary>
        /// <param name="name">Name of the type alias.</param>
        /// <param name="dataType">The type being aliased.</param>
        public TypeReference(string name, DataType dataType) : base(dataType.Domain, name)
        {
            this.referent = dataType;
        }

        /// <summary>
        /// This constructor is used to generate generic type variables.
        /// </summary>
        /// <param name="name">The name of the generic type variable.</param>
        public TypeReference(string name) : base(Domain.Any, name)
        {
            // This kind of type reference is used as a placeholder, and should never
            // participate in Reko's type inference.
            this.referent = default!;
        }

        /// <inheritdoc/>
        public override bool IsComplex => Referent.IsComplex;

        /// <inheritdoc/>
        public override bool IsIntegral => Referent.IsIntegral;

        /// <inheritdoc/>
        public override bool IsPointer => Referent.IsPointer;

        /// <inheritdoc/>
        public override bool IsReal => Referent.IsReal;

        /// <summary>
        /// The type being aliased.
        /// </summary>
        public DataType Referent
        {
            get { return referent; }
            set { referent = value; Domain = referent.Domain; }
        }

        /// <inheritdoc/>
        public override int Size
        {
            get { return Referent.Size; }
            set { ThrowBadSize(); }
        }

        /// <inheritdoc/>
        public override void Accept(IDataTypeVisitor v)
        {
            v.VisitTypeReference(this);
        }

        /// <inheritdoc/>
        public override T Accept<T>(IDataTypeVisitor<T> v)
        {
            return v.VisitTypeReference(this);
        }

        /// <inheritdoc/>
        public override DataType Clone(IDictionary<DataType, DataType>? clonedTypes)
        {
            return this;
        }
    }
}
