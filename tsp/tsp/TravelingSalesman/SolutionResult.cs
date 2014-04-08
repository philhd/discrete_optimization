using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System;

namespace TravelingSalesman
{
    /// <summary>
    ///   A solution result for the tsp problem
    /// </summary>
    public class SolutionResult
    {
        private readonly List<Node> path = new List<Node>();

        private Random rand = new Random(5);

        public SolutionResult()
        {
            this.Edges = new List<Edge>();
            this.Nodes = new List<Node>();
        }

        /// <summary>
        ///   Gets the total distance of the path.
        /// </summary>
        /// <value> The distance. </value>
        public double GetDistance()
        {
            //double distance = 0;
            //foreach (Edge edge in this.Edges)
            //{
            //    distance += edge.Length;
            //}

            //return distance;

            // get the last leg distance
            var distance = path.Last().DistanceTo(path.First());
            for (int k = 1; k < path.Count; k++)
            {
                // add in all the intermediate distances
                distance += path[k].DistanceTo(path[k - 1]);
            }
            return distance;

        }

        /// <summary>
        ///   Gets the path.
        /// </summary>
        /// <value> The path. </value>
        public List<Node> Path
        {
            get { return path; }
        }

        public List<Edge> Edges { get; set; }

        public List<Node> Nodes { get; set; }

        public void Swap(int node1, int node2)
        {
            if (node1 > node2)
            {
                int temp = node2;
                node2 = temp;
                node2 = node1;
                node1 = temp;
            }
            this.Path.Reverse(node1, (Math.Abs(node2 - node1) + 1));
        }

        public int getRandomNodeIndex(int indexToExclude)
        {
            int index = -1;
            while (index == -1)
            {
                int randIndex = this.rand.Next(this.Path.Count);
                if (randIndex != indexToExclude)
                {
                    index = randIndex;
                }
            }

            return index;
        }

        /// <summary>
        ///   Returns a <see cref="System.String" /> that represents the current <see cref="SolutionResult" /> . This conforms to the output format expected in the course.
        /// </summary>
        /// <returns> A <see cref="System.String" /> that represents the current <see cref="SolutionResult" /> . </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(this.GetDistance().ToString(CultureInfo.InvariantCulture) + " 0");

            foreach (var node in Path)
                builder.Append(node.Index + " ");

            builder.AppendLine("");
            return builder.ToString();
        }

        public string ShowEdges()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Edge edge in this.Edges)
            {
                sb.AppendLine(edge.ToString());
            }
            return sb.ToString();
        }
    }
}