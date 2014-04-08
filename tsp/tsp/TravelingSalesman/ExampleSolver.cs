using System.Collections.Generic;
using System.Linq;

namespace TravelingSalesman
{
    /// <summary>
    ///   A simple example solver (the same as the one which comes with the assignment)
    /// </summary>
    public class ExampleSolver : ISolver
    {
        #region ISolver Members

        public SolutionResult Solve(IEnumerable<Node> nodes)
        {
            var result = new SolutionResult();

            foreach (var node in nodes)
            {
                if(result.Path.Count > 0)
                {
                    result.Edges.Add(new Edge(result.Nodes.Last(),node));
                }
                result.Path.Add(node);
                result.Nodes.Add(node);

            }

            // complete the cycle
            result.Edges.Add(new Edge(result.Nodes.Last(), result.Nodes.First()));

            return result;
        }

        #endregion
    }
}