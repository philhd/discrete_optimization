using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    /// <summary>
    ///   A solution result for the tsp problem
    /// </summary>
    public class SolutionResult
    {
        private readonly List<Node> path = new List<Node>();

        /// <summary>
        ///   Gets the total distance of the path.
        /// </summary>
        /// <value> The distance. </value>
        public double Distance
        {
            get
            {
                // get the last leg distance
                var distance = path.Last().DistanceTo(path.First());
                for (int k = 1; k < path.Count; k++)
                {
                    // add in all the intermediate distances
                    distance += path[k].DistanceTo(path[k - 1]);
                }
                return distance;
            }
        }

        /// <summary>
        ///   Gets the path.
        /// </summary>
        /// <value> The path. </value>
        public List<Node> Path
        {
            get { return path; }
        }

        /// <summary>
        ///   Returns a <see cref="System.String" /> that represents the current <see cref="SolutionResult" /> . This conforms to the output format expected in the course.
        /// </summary>
        /// <returns> A <see cref="System.String" /> that represents the current <see cref="SolutionResult" /> . </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Distance.ToString(CultureInfo.InvariantCulture) + " 0");

            foreach (var node in Path)
                builder.Append(node.Index + " ");

            builder.AppendLine("");
            return builder.ToString();
        }
    }
}