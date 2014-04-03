using System.Collections.Generic;

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
                result.Path.Add(node);

            return result;
        }

        #endregion
    }
}