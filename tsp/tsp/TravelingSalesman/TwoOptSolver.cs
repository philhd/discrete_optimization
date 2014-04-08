using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class TwoOptSolver : ISolver
    {
        public TwoOptSolver()
        {
            this.Graph = new TspGraph();
        }

        public TspGraph Graph { get; private set; }

        public TspGraph Solve(IEnumerable<Node> nodes)
        {
            int numIterations = 10000;
            var result = new TspGraph();

            foreach (var node in nodes)
                result.Path.Add(node);

            for (int i = 0; i < numIterations; i++)
            {
                double prevDistance = result.GetDistance();
                int node1 = result.getRandomNodeIndex(-1);
                int node2 = result.getRandomNodeIndex(node1);
                result.Swap(node1, node2);
                double newDistance = result.GetDistance();
                if (newDistance > prevDistance)
                {
                    result.Swap(node1, node2);
                }
                else
                {
                    //Console.WriteLine(string.Format("swapped {0} with {1}. distance {2} -> {3}", node1, node2, prevDistance, newDistance));
                }
            }

            return result;
        }
    }
}
