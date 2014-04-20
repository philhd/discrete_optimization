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
    public class TspGraph
    {
        private readonly List<Node> path = new List<Node>();

        private Random rand = new Random(5);

        public event Action<Node> NodeAdded;

        public event Action<Edge> EdgeUpdated;

        public event Action RenderNeeded;

        public event Action<string> outputAdded;

        public TspGraph()
        {
            this.Edges = new List<Edge>();
            this.Nodes = new List<Node>();
        }

        /// <summary>
        ///   Gets the total distance of the path.
        /// </summary>
        /// <value> The distance. </value>
        public double GetDistanceEdges()
        {
            double distance = 0;
            foreach (Edge edge in this.Edges)
            {
                distance += edge.Length;
            }

            return distance;
        }

        public double GetDistance()
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

        public void AddNode(Node node)
        {
            this.Nodes.Add(node);
            this.Path.Add(node);
            this.RaiseNodeAdded(node);
        }

        public void AddEdge(Edge edge)
        {
            this.Edges.Add(edge);
            this.RaiseEdgeUpdated(edge);
        }

        public void Swap(int node1, int node2)
        {
            if (node1 > node2)
            {
                int temp = node2;
                node2 = node1;
                node1 = temp;
            }
            this.Path.Reverse(node1, (Math.Abs(node2 - node1) + 1));
        }

        public void SwapEdges(int edge1, int edge2)
        {
            if (edge1 > edge2)
            {
                int temp = edge2;
                edge2 = edge1;
                edge1 = temp;
            }
            // swap the end node of the lower index node with the start node of the higher index edge
            // ex. [1-2,2-3,3-4,4-5,5-1] would become [1-4,2-3,3-4,2-5,5-1]
            Node tempNode = this.Edges[edge2].Start;
            this.Edges[edge2].Start = this.Edges[edge1].End;
            this.Edges[edge1].End = tempNode;
            
            // recalculate new edge lengths
            this.Edges[edge1].RecalcLength();
            this.Edges[edge2].RecalcLength();

            // fire update events
            //this.RaiseRenderNeeded();
            this.RaiseEdgeUpdated(this.Edges[edge1]);
            this.RaiseEdgeUpdated(this.Edges[edge2]);

            // now reverse the subsequence of edges affected (all the ones in between the two edges chosen)
            // ex. from here [1-4,2-3,3-4,2-5,5-1], we reverse edges 2-3 and 3-4
            int startIndex = edge1 + 1;
            int count = Math.Abs(edge2 - edge1) - 1;
            this.Edges.Reverse(startIndex, count);

            // now swap the starts and ends of the edges we just reversed to regain a tour
            for (int i = startIndex; i < edge2; i++)
            {
                this.Edges[i].ReverseDirection();
            }
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
        ///   Returns a <see cref="System.String" /> that represents the current <see cref="TspGraph" /> . This conforms to the output format expected in the course.
        /// </summary>
        /// <returns> A <see cref="System.String" /> that represents the current <see cref="TspGraph" /> . </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(this.GetDistance().ToString(CultureInfo.InvariantCulture) + " 0");

            foreach (var node in Path)
                builder.Append(node.Index + " ");

            builder.AppendLine("");
            return builder.ToString();
        }

        public string ToStringEdges()
        {
            var builder = new StringBuilder();
            builder.AppendLine(this.GetDistanceEdges().ToString(CultureInfo.InvariantCulture) + " 0");

            foreach (var edge in this.Edges)
            {
                builder.Append(edge.Start.Index + " ");
            }

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

        private void RaiseNodeAdded(Node node)
        {
            if (this.NodeAdded != null)
            {
                this.NodeAdded(node);
            }
        }

        private void RaiseEdgeUpdated(Edge edge)
        {
            if (this.EdgeUpdated != null)
            {
                this.EdgeUpdated(edge);
            }
        }

        private void RaiseRenderNeeded()
        {
            if (this.RenderNeeded != null)
            {
                this.RenderNeeded();
            }
        }
    }
}