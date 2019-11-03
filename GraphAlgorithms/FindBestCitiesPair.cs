using System;
using System.Collections.Generic;
using System.Linq;
using ASD.Graphs;

namespace Lab7
{
    public class BestCitiesSolver : MarshalByRefObject
    {
        public (int c1, int c2, int? bypass, double time, Edge[] path)? FindBestCitiesPair(Graph times, double[] passThroughCityTimes, int[] nominatedCities, bool buildBypass)
        {
            double min = double.MaxValue;
            PathsInfo[][] tab = new PathsInfo[times.VerticesCount][];
            int a = -1;
            int b = -1;
            PathsInfo[] best = new PathsInfo[times.VerticesCount];

            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(true, times.VerticesCount);
            for (int j = 0; j < times.VerticesCount; ++j)
            {
                foreach (var e in times.OutEdges(j))
                {
                    g.AddEdge(e.From, e.To, e.Weight + passThroughCityTimes[j]);
                }                
            }

            for (int i1 = 0; i1 < nominatedCities.Length; ++i1)
            {
                int i = nominatedCities[i1];
                foreach(var e in g.OutEdges(i))
                {
                    g.DelEdge(e);
                    g.AddEdge(e.From, e.To, e.Weight - passThroughCityTimes[i]);
                }
                g.DijkstraShortestPaths(i, out tab[i]);
                for (int j = 0; j < g.VerticesCount; ++j)
                {
                    if (j != i && tab[i][j].Dist <= min && nominatedCities.Contains(j))
                    {
                        min = tab[i][j].Dist;
                        a = i;
                        b = j;
                    }
                }
            }

            double minTmp = double.MaxValue;
            int x1 = -1, y1 = -1, k1 = -1;
            if (buildBypass)
            {
                for (int i = 0; i < nominatedCities.Length; ++i)
                {
                    int x = nominatedCities[i];
                    if (times.OutDegree(x) == 0)
                        continue;
                    for (int j = i+1; j < nominatedCities.Length; ++j)
                    {
                        int y = nominatedCities[j];
                        if (times.OutDegree(y)==0)
                            continue;
                        for (int k = 0; k < times.VerticesCount; ++k)
                        {
                            if (tab[x][k].Dist + tab[y][k].Dist < minTmp)
                            {
                                minTmp = tab[x][k].Dist + tab[y][k].Dist;
                                x1 = x;
                                y1 = y;
                                k1 = k;
                            }
                        }
                    }
                }
            }

            if (a == -1 || b == -1 || min == double.MaxValue)
                return null;

            if (minTmp < min)
            {
                List<Edge> lst = new List<Edge>();
                int ind = k1;
                best = tab[x1];
                while (best[ind].Last.HasValue)
                {
                    Edge e = best[ind].Last.Value;
                    lst.Insert(0, e);
                    ind = e.From;
                }
                ind = k1;
                best = tab[y1];
                while (best[ind].Last.HasValue)
                {
                    Edge e = best[ind].Last.Value;
                    lst.Add(new Edge(e.To, e.From, e.Weight));
                    ind = e.From;
                }
                Edge[] path = lst.ToArray();
                return (x1, y1, k1, minTmp, path);
            }
            else
            {
                List<Edge> lst = new List<Edge>();
                int ind = b;
                best = tab[a];
                while (best[ind].Last.HasValue)
                {
                    Edge e = best[ind].Last.Value;
                    lst.Insert(0, e);
                    ind = e.From;
                }
                Edge[] path = lst.ToArray();
                return (a, b, null, min, path);
            }
        }
    }

}

