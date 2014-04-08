using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class TwoOptSolver : ISolver
    {
        public event Action DataComplete;

        public TwoOptSolver()
        {
            this.Graph = new TspGraph();
        }

        public TspGraph Graph { get; private set; }

        public TspGraph Solve(IEnumerable<Node> nodes)
        {
            int numIterations = 1;

            foreach (var node in nodes)
                this.Graph.AddNode(node);

            this.RaiseDataComplete();

            for (int i = 0; i < numIterations; i++)
            {
                double prevDistance = this.Graph.GetDistance();
                int node1 = this.Graph.getRandomNodeIndex(-1);
                int node2 = this.Graph.getRandomNodeIndex(node1);
                this.Graph.Swap(node1, node2);
                double newDistance = this.Graph.GetDistance();
                if (newDistance > prevDistance)
                {
                    this.Graph.Swap(node1, node2);
                }
                else
                {
                    //Console.WriteLine(string.Format("swapped {0} with {1}. distance {2} -> {3}", node1, node2, prevDistance, newDistance));
                }
            }

            return this.Graph;
        }

        private void RaiseDataComplete()
        {
            if (this.DataComplete != null)
            {
                this.DataComplete();
            }
        }
    }
}
