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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Reko.Core.Graphs
{
    /// <summary>
    /// Simple implementation of <see cref="DirectedGraph{T}"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DiGraph<T> : DirectedGraph<T>
            where T : notnull
    {
        private readonly Dictionary<T, Node> nodes;
        private readonly NodeCollection nodeCollection;

        public DiGraph()
        {
            this.nodes = new Dictionary<T, Node>();
            this.nodeCollection = new NodeCollection(this);
        }

        private class Node
        {
            public T value;
            public List<Node> Predecessors;
            public List<Node> Successors;
            public ItemCollection Pred;
            public ItemCollection Succ;

            public Node(T item)
            {
                value = item;
                Predecessors = new List<Node>(2);
                Successors = new List<Node>(2);
                Pred = new ItemCollection(Predecessors);
                Succ = new ItemCollection(Successors);
            }
        }

        private class ItemCollection : ICollection<T>
        {
            private readonly List<Node> outer;

            public ItemCollection(List<Node> outer)
            {
                this.outer = outer;
            }

            public void Add(T item)
            {
                throw new NotImplementedException();
            }

            public void Clear()
            {
                throw new NotImplementedException();
            }

            public bool Contains(T item)
            {
                return outer.Select(n => n.value).Contains(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                for (int i = 0; i < outer.Count; ++i)
                {
                    array[i + arrayIndex] = outer[i].value;
                }
            }

            public int Count
            {
                get { return outer.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(T item)
            {
                throw new NotImplementedException();
            }

            public IEnumerator<T> GetEnumerator()
            {
                return outer.Select(n => n.value).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private class NodeCollection : ICollection<T>
        {
            private readonly DiGraph<T> outer;

            public NodeCollection(DiGraph<T> outer)
            {
                this.outer = outer;
            }

            public void Add(T item)
            {
                outer.AddNode(item);
            }

            public void Clear()
            {
                outer.nodes.Clear();
            }

            public bool Contains(T item)
            {
                return outer.nodes.ContainsKey(item);
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                outer.nodes.Keys.CopyTo(array, arrayIndex);
            }

            public int Count
            {
                get { return outer.nodes.Keys.Count; }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool Remove(T item)
            {
                return outer.RemoveNode(item);
            }

            public IEnumerator<T> GetEnumerator()
            {
                return outer.nodes.Keys.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        public ICollection<T> Predecessors(T item)
        {
            return nodes[item].Pred;
        }

        public ICollection<T> Successors(T item)
        {
            return nodes[item].Succ;
        }

        public ICollection<T> Nodes
        {
            get { return nodeCollection; }
        }

        public void AddNode(T item)
        {
            if (!nodes.ContainsKey(item))
            {
                var node = new Node(item);
                nodes.Add(item, node);
            }
        }

        public bool RemoveNode(T item)
        {
            if (!nodes.TryGetValue(item, out Node? node))
            {
                return false;
            }
            for (int i = node.Predecessors.Count-1; i >= 0; --i)
            {
                node.Predecessors[i].Successors.RemoveAll(n => n == node);
            }
            for (int i = node.Successors.Count - 1; i >= 0; --i)
            {
                node.Successors[i].Predecessors.RemoveAll(n => n == node);
            }
            nodes.Remove(item);
            return true;
        }

        public void AddEdge(T itemFrom, T itemTo)
        {
            var nFrom = nodes[itemFrom];
            var nTo = nodes[itemTo];
            nFrom.Successors.Add(nTo);
            nTo.Predecessors.Add(nFrom);
        }

        public void RemoveEdge(T itemFrom, T itemTo)
        {
            var nFrom = nodes[itemFrom];
            var nTo = nodes[itemTo];
            int iSucc = nFrom.Successors.IndexOf(nTo);
            int iPred = nTo.Predecessors.IndexOf(nFrom);
            if (iSucc >= 0 && iPred >= 0)
            {
                nFrom.Successors.RemoveAt(iSucc);
                nTo.Predecessors.RemoveAt(iPred);
            }
        }

        public bool ContainsEdge(T itemFrom, T itemTo)
        {
            var nFrom = nodes[itemFrom];
            var nTo = nodes[itemTo];
            int iSucc = nFrom.Successors.IndexOf(nTo);
            int iPred = nTo.Predecessors.IndexOf(nFrom);
            return (iSucc >= 0 && iPred >= 0);
        }

        public override string ToString()
        {
            return string.Format("Nodes = {0}", nodes.Count);
        }
    }

    /// <summary>
    /// Directed graph with edge annotations
    /// </summary>
    public class DiGraph<V, E> : DirectedGraph<V, E>
        where V : notnull
    {
        private readonly Dictionary<V, List<(V, E)>> inEdges;
        private readonly Dictionary<V, List<(V, E)>> outEdges;
        private readonly IEqualityComparer<V> cmp;

        public DiGraph()
        {
            this.Nodes = new HashSet<V>();
            this.inEdges = new Dictionary<V, List<(V, E)>>();
            this.outEdges = new Dictionary<V, List<(V, E)>>();
            this.cmp = EqualityComparer<V>.Default;
        }

        public ICollection<V> Nodes { get; }


        public void AddEdge(V nodeFrom, V nodeTo, E edgeData)
        {
            if (!outEdges.TryGetValue(nodeFrom, out var succs))
            {
                succs = new List<(V, E)>();
                outEdges.Add(nodeFrom, succs);
            }
            succs.Add((nodeTo, edgeData));
            if (!inEdges.TryGetValue(nodeTo, out var preds))
            {
                preds = new List<(V, E)>();
                inEdges.Add(nodeTo, preds);
            }
            preds.Add((nodeFrom, edgeData));
        }

        public bool ContainsEdge(V nodeFrom, V nodeTo)
        {
            if (!outEdges.TryGetValue(nodeFrom, out var succs))
                return false;
            if (succs.FindIndex(s => cmp.Equals(s.Item1, nodeTo)) < 0)
                return false;
            if (!inEdges.TryGetValue(nodeTo, out var preds))
                return false;
            return (preds.FindIndex(p => cmp.Equals(p.Item1, nodeFrom)) >= 0);
        }

        public ICollection<(V, E)> Predecessors(V node)
        {
            if (!inEdges.TryGetValue(node, out var preds))
                return Array.Empty<(V, E)>();
            return preds;
        }

        public void RemoveEdge(V nodeFrom, V nodeTo)
        {
            if (outEdges.TryGetValue(nodeFrom, out var succs))
            {
                var idx = succs.FindIndex(s => cmp.Equals(s.Item1, nodeTo));
                if (idx < 0)
                    return;
                succs.RemoveAt(idx);
            }
            if (inEdges.TryGetValue(nodeTo, out var preds))
            {
                var idx = preds.FindIndex(p => cmp.Equals(p.Item1, nodeFrom));
                if (idx < 0)
                    return;
                preds.RemoveAt(idx);
            }
        }

        public ICollection<(V, E)> Successors(V node)
        {
            if (!inEdges.TryGetValue(node, out var preds))
                return Array.Empty<(V, E)>();
            return preds;
        }
    }
}
