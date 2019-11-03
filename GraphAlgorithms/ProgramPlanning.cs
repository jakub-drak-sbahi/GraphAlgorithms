using System;
using System.Linq;
using ASD.Graphs;
using System.Collections.Generic;

namespace Lab13
{
    public class ProgramPlanning : MarshalByRefObject
    {
        public double CalculateTimesLatestPossible(Graph taskGraph, double[] taskTimes, out double[] startTimes, out int[] criticalPath)
        {
            int[] o2t;
            int[] t2o;
            taskGraph.TopologicalSort(out o2t, out t2o);
            int n = taskTimes.Length;
            double worstTime = 0;
            int worstInd = 0;
            int[] last = new int[n];
            double[] worstTimes1 = new double[n];
            for (int i = 0; i < n; ++i)
            {
                worstTimes1[i] = taskTimes[i];
                last[i] = -1;
                if (worstTimes1[i] > worstTime)
                {
                    worstTime = worstTimes1[i];
                    worstInd = i;
                }
            }

            for (int i = 0; i < n; ++i)
            {
                foreach (var e in taskGraph.OutEdges(t2o[i]))
                {
                    if (worstTimes1[e.To] < worstTimes1[e.From] + taskTimes[e.To])
                    {
                        worstTimes1[e.To] = worstTimes1[e.From] + taskTimes[e.To];
                        last[e.To] = e.From;
                    }
                }
                if (worstTimes1[t2o[i]] > worstTime)
                {
                    worstTime = worstTimes1[t2o[i]];
                    worstInd = t2o[i];
                }
            }

            List<int> criticalList = new List<int>();
            int ind = worstInd;

            while (ind != -1)
            {
                criticalList.Add(ind);
                ind = last[ind];
            }

            criticalList.Reverse();
            criticalPath = criticalList.ToArray();
            startTimes = new double[n];
            for (int i = 0; i < n; ++i)
                startTimes[i] = worstTime - taskTimes[i];
            for (int i = n - 1; i >= 0; --i)
            {
                foreach(var e in taskGraph.OutEdges(t2o[i]))
                {
                    if (startTimes[e.From] > startTimes[e.To] - taskTimes[e.From])
                        startTimes[e.From] = startTimes[e.To] - taskTimes[e.From];
                }
            }
            return worstTime;
        }
    }
}
