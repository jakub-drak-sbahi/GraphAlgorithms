using System;
using System.Collections.Generic;
using System.Linq;
using ASD.Graphs;

namespace Lab9
{
    public class DistributionFinder : MarshalByRefObject
    {
        public (int satisfactionLevel, int[] bestDistribution) FindBestDistribution(int[] limits, int[][] preferences, bool[] isSportActivity)
        {
            int k = limits.Length;
            int n = preferences.GetLength(0);
            int wejscie = k + n;
            int wyjscie = k + n + 1;

            Graph g = new AdjacencyListsGraph<HashTableAdjacencyList>(true, k + n + 2);
            for(int i=0; i<k; ++i)
            {
                if(isSportActivity==null || isSportActivity[i])
                {
                    g.AddEdge(i, wyjscie, limits[i]);
                }
            }
            for(int i=k; i<k+n; ++i)
            {
                g.AddEdge(wejscie, i, 1);
                foreach(var v in preferences[i-k])
                {
                    if (isSportActivity == null || isSportActivity[v])
                    {
                        g.AddEdge(i, v, 1);
                    }
                }
            }

            double p;
            Graph ret;
            (p, ret) = g.FordFulkersonDinicMaxFlow(wejscie, wyjscie, MaxFlowGraphExtender.OriginalDinicBlockingFlow);

            if (isSportActivity != null)
            {
                int sum = 0;
                for (int i = 0; i < k; ++i)
                {
                    if (isSportActivity[i])
                    {
                        sum += limits[i];
                    }
                }
                if (sum == p)
                {
                    p = 1;
                }
                else
                {
                    return (0, null);
                }
            }

            int[] bestDistribution = new int[n];
            for(int i=0; i<n; ++i)
            {
                bestDistribution[i] = -1;
                foreach(var e in ret.OutEdges(i+k))
                {
                    if(e.Weight==1)
                    {
                        bestDistribution[i] = e.To;
                        break;
                    }
                }
            }
            return ((int)p, bestDistribution);
        }
    }
}