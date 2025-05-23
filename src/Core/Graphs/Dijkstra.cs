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

using Reko.Core.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reko.Core.Graphs
{
    /// <summary>
    /// Implements Dijkstra's shortest-path algorithm for a directed graph.
    /// </summary>
    /// <typeparam name="T">Node type of the graph.</typeparam>
    public class Dijkstra<T>
        where T : notnull
    {
        private Dictionary<T, double> dist;
        private Dictionary<T, T> prev;

        private Dijkstra(Dictionary<T, double> dist)
        {
            this.dist = dist;
            this.prev = new Dictionary<T, T>();
        }

        /// <summary>
        /// Implementation of the Dijkstra's shortest-path algorithm. Given a <paramref name="graph"/>
        /// it computes the shortest paths that can be reached starting at <paramref name="source"/>.
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="source"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public static Dijkstra<T> ShortestPath(DirectedGraph<T> graph, T source, Func<T, T, double> weight)
        {
            var Q = new FibonacciHeap<double, T>();
            var q = new HashSet<T>(graph.Nodes);
            var self = new Dijkstra<T>(graph.Nodes.ToDictionary(K => K, V => double.PositiveInfinity));

            self.dist[source] = 0;                        // Distance from source to source


            foreach (var v in graph.Nodes)
            {
                Q.Insert(v, self.dist[v]);
            }

            while (!Q.IsEmpty)
            {
                var u = Q.removeMin();                            // Remove and return best vertex
                q.Remove(u);
                foreach (var v in graph.Successors(u))
                {
                    // only v that is still in Q
                    var alt = self.dist[u] + weight(u, v);
                    if (alt < self.dist[v])
                    {
                        self.dist[v] = alt;
                        self.prev[v] = u;
                        Q.decreaseKey(v, alt);
                    }
                }
            }
            return self;
        }

        /// <summary>
        /// The shortest path to the destination.
        /// </summary>
        /// <param name="destination">Destination for which we
        /// want a path.</param>
        /// <returns>A list of nodes to the destination.</returns>
        public List<T> GetPath(T destination)
        {
            var path = new List<T>();
            while (prev.TryGetValue(destination, out T? p))
            {
                path.Add(p);
                destination = p;
            }
            path.Reverse();
            return path;
        }
    }
}
