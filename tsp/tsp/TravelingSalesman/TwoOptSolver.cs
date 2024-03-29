﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class TwoOptSolver : SolverBase
    {
        public TwoOptSolver()
        {
            this.Graph = new TspGraph();
            this.MoveDecider = new MoveDeciderBasic();
        }

        public override TspGraph Solve(IEnumerable<Node> nodes)
        {
            int numIterations = 100000;

            // initial tour is completely greedy
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
                if (!this.MoveDecider.ShouldMakeMove(prevDistance, newDistance))
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
    }
}
