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

using Reko.Core;
using Reko.Core.Graphs;
using System;
using System.Collections.Generic;

namespace Reko.Analysis
{
    /// <summary>
    /// This class is a wrapper for the <see cref="CallGraph"/> of a program.
    /// The edges go from calling procedure to called procedure.
    /// </summary>
    public class ProcedureGraph : DirectedGraph<Procedure>
    {
        private readonly CallGraph cg;

        public ProcedureGraph(Program program)
        {
            this.cg = program.CallGraph;
            this.Nodes = program.Procedures.Values;
        }

        public ICollection<Procedure> Nodes { get; }

        public ICollection<Procedure> Predecessors(Procedure node)
        {
            throw new NotSupportedException();
        }

        public ICollection<Procedure> Successors(Procedure node)
        {
            var succs = new List<Procedure>(cg.Callees(node));
            return succs;
        }

        public void AddEdge(Procedure nodeFrom, Procedure nodeTo)
        {
            throw new NotSupportedException();
        }

        public void RemoveEdge(Procedure nodeFrom, Procedure nodeTo)
        {
            throw new NotSupportedException();
        }

        public bool ContainsEdge(Procedure nodeFrom, Procedure nodeTo)
        {
            throw new NotSupportedException();
        }
    }
}
