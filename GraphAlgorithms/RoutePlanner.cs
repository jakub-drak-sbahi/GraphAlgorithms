using System;
using System.Collections.Generic;
using ASD.Graphs;
using System.Linq;

namespace ASD
{
    public class RoutePlanner : MarshalByRefObject
    {
        public int[] FindCycle(Graph g)
        {
            bool ret = true;
            int a, last = 0;
            bool[] visitedFrom = new bool[g.VerticesCount];
            bool[] visitedTo = new bool[g.VerticesCount];
            int[] previous = new int[g.VerticesCount];
            List<int> cycle = new List<int>();
            Predicate<int> PreVisit = delegate (int n)
            {
                visitedFrom[n] = true;
                return true;
            };
            Predicate<int> PostVisit = delegate (int n)
            {
                visitedFrom[n] = visitedTo[n] = false;
                return true;
            };
            Predicate<Edge> EdgeFun = delegate (Edge e)
            {
                if (visitedFrom[e.To])
                {
                    last = e.To;
                    ret = false;
                }
                else
                {
                    visitedFrom[e.From] = visitedTo[e.To] = true;
                }
                previous[e.To] = e.From;
                return ret;
            };
            g.GeneralSearchAll<EdgesStack>(PreVisit, PostVisit, EdgeFun, out a);
            if (ret)
                return null;
            int it = last;
            while (previous[it] != last)
            {
                cycle.Add(previous[it]);
                it = previous[it];
            }
            cycle.Add(last);
            cycle.Reverse();
            return cycle.ToArray();
        }

        public int[][] FindShortRoutes(Graph g)
        {
            for (int i = 0; i < g.VerticesCount; ++i)
                if (g.InDegree(i) != g.OutDegree(i))
                    return null;
            int[] tab;
            List<int[]> ret = new List<int[]>();
            Graph tmp = g.Clone();
            tab = FindCycle(g);
            if (tab == null)
                return null;
            while ((tab = FindCycle(tmp)) != null)
            {
                ret.Add(tab);
                for (int i = 0; i < tab.Length - 1; ++i)
                {
                    tmp.DelEdge(tab[i], tab[i + 1]);
                }
                tmp.DelEdge(tab[tab.Length - 1], tab[0]);
            }
            return ret.ToArray();
        }

        public int[][] FindLongRoutes(Graph g)
        {
            int[][] tab = FindShortRoutes(g);
            if (tab == null)
                return null;
            int n = g.VerticesCount;
            int[] mapa = new int[tab.GetLength(0)];
            for (int i = 0; i < mapa.Length; ++i)
                mapa[i] = i;

            List<int>[] nal = new List<int>[n];
            for (int i = 0; i < n; ++i)
                nal[i] = new List<int>();
            for (int i = 0; i < tab.GetLength(0); ++i)
            {
                for (int j = 0; j < tab[i].Length; ++j)
                {
                    nal[tab[i][j]].Add(i);
                }
            }

            for (int i = 0; i < n; ++i)
            {
                while (nal[i].Count > 1)
                {
                    if (mapa[nal[i][0]] == mapa[nal[i][1]])
                    {
                        nal[i].RemoveAt(0);
                        continue;
                    }
                    int from = mapa[nal[i][0]];
                    int to = mapa[nal[i][1]];
                    int size = tab[from].Length + tab[to].Length;
                    int[] tmp = new int[size];
                    int indFrom;
                    for (indFrom = 0; tab[from][indFrom] != i; indFrom++) ;
                    int indTo;
                    for (indTo = 0; tab[to][indTo] != i; indTo++) ;
                    for (int p = 0; p < tab[from].Length; ++p, ++indFrom)
                        tmp[p] = tab[from][indFrom % tab[from].Length];
                    for (int p = tab[from].Length; p < size; ++p, ++indTo)
                        tmp[p] = tab[to][indTo % tab[to].Length];
                    tab[to] = tmp;
                    tab[from] = null;
                    mapa[from] = mapa[to];
                    for (int k = 0; k < mapa.Length; ++k)
                    {
                        if (mapa[k] != mapa[mapa[k]])
                        {
                            mapa[k] = mapa[mapa[k]];
                        }
                    }
                    nal[i].RemoveAt(0);
                }
            }

            List<int[]> lst = new List<int[]>();
            for (int i = 0; i < tab.GetLength(0); ++i)
            {
                if (tab[i] != null)
                {
                    lst.Add(tab[i]);
                }
            }
            return lst.ToArray();
        }
    }
}
