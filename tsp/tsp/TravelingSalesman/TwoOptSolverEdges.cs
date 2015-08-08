using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class TwoOptSolverEdges : SolverBase
    {
        public TwoOptSolverEdges()
        {
            this.Graph = new TspGraph();
            this.MoveDecider = new MoveDeciderSA();
        }

        public override TspGraph Solve(IEnumerable<Node> nodes)
        {
            int numIterations = 10000;

            // initial tour is completely greedy
            List<Node> nodeList = nodes.ToList();
            for (int i = 0; i < nodeList.Count; i++)
            {
                //add node
                this.Graph.AddNode(nodeList[i]);
                //add edge
                if (i == nodeList.Count - 1)
                {
                    // complete the cycle
                    this.Graph.AddEdge(new Edge(nodeList[i], nodeList[0]));
                }
                else
                {
                    this.Graph.AddEdge(new Edge(nodeList[i], nodeList[i + 1]));
                }
            }

            //Console.WriteLine(this.Graph.ShowEdges());
            this.RaiseDataComplete();

            this.LocalSearch(numIterations);

            return this.Graph;
        }

        public override string SolverInfo
        {
            get
            {
                return string.Format("{0} Distance={1} ", base.SolverInfo, this.Graph.Distance);
            }
        }

        private void LocalSearch(int numIterations)
        {
            for (int i = 0; i < numIterations; i++)
            {
                double prevDistance = this.Graph.GetDistanceEdges();
                int node1 = this.Graph.getRandomNodeIndex(-1);
                int node2 = this.Graph.getRandomNodeIndex(node1);
                this.Graph.SwapEdges(node1, node2);
                double newDistance = this.Graph.GetDistanceEdges();
                if (!this.MoveDecider.ShouldMakeMove(prevDistance, newDistance))
                {
                    this.Graph.SwapEdges(node1, node2);
                }
                else
                {
                    this.Graph.Distance = newDistance;
                    //Console.WriteLine(string.Format("swapped {0} with {1}. distance {2} -> {3}", node1, node2, prevDistance, newDistance));
                    //Console.WriteLine(this.Graph.ShowEdges());
                }

                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
