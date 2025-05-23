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

using Reko.Core.Operators;
using Reko.Core.Types;
using System.Collections.Generic;

namespace Reko.Core.Expressions
{
    /// <summary>
    /// Models a unary expression like logical negation or bit complement.
    /// </summary>
	public class UnaryExpression : AbstractExpression
	{
        /// <summary>
        /// Creates an instance of the <see cref="UnaryExpression"/> class.
        /// </summary>
        /// <param name="op">The unary operation.</param>
        /// <param name="type">The result of the unary operation.</param>
        /// <param name="expr">The expression being operated on.</param>
		public UnaryExpression(UnaryOperator op, DataType type, Expression expr) : base(type)
		{
			this.Operator = op; this.Expression = expr;
		}

        /// <summary>
        /// The operator for the unary expression.
        /// </summary>
        public UnaryOperator Operator { get; }

        /// <summary>
        /// The operand of the expression.
        /// </summary>
        public Expression Expression { get; }

        /// <inheritdoc/>
        public override IEnumerable<Expression> Children
        {
            get { yield return Expression; }
        }

        /// <inheritdoc/>
        public override T Accept<T, C>(ExpressionVisitor<T, C> v, C context)
        {
            return v.VisitUnaryExpression(this, context);
        }

        /// <inheritdoc/>
        public override T Accept<T>(ExpressionVisitor<T> v)
        {
            return v.VisitUnaryExpression(this);
        }

        /// <inheritdoc/>
		public override void Accept(IExpressionVisitor v)
		{
			v.VisitUnaryExpression(this);
		}

        /// <inheritdoc/>
		public override Expression CloneExpression()
		{
            return new UnaryExpression(Operator, DataType.Clone(), Expression.CloneExpression());
		}

        /// <inheritdoc/>
		public override Expression Invert()
        {
			if (Operator == Operators.Operator.Not)
				return Expression;
			else
                return new UnaryExpression(Operators.Operator.Not, PrimitiveType.Bool, this);
		}
	}
}
