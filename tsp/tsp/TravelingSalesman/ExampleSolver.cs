using System.Collections.Generic;
using System.Linq;
using System;

namespace TravelingSalesman
{
    /// <summary>
    ///   A simple example solver (the same as the one which comes with the assignment)
    /// </summary>
    public class ExampleSolver : SolverBase
    {
        #region ISolver Members

        public ExampleSolver()
        {
            this.Graph = new TspGraph();
        }

        public override TspGraph Solve(IEnumerable<Node> nodes)
        {
            foreach (var node in nodes)
            {
                if (this.Graph.Path.Count > 0)
                {
                    this.Graph.Edges.Add(new Edge(this.Graph.Nodes.Last(), node));
                }
                //this.Graph.Path.Add(node);
                this.Graph.AddNode(node);
            }

            // complete the cycle
            this.Graph.Edges.Add(new Edge(this.Graph.Nodes.Last(), this.Graph.Nodes.First()));

            return this.Graph;
        }

        #endregion

        public string SolverInfo
        {
            get { throw new NotImplementedException(); }
        }
    }
}