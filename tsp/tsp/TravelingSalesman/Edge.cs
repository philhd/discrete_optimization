using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class Edge
    {
        public Edge(Node startNode, Node endNode)
        {
            this.StartNode = startNode;
            this.EndNode = endNode;
            this.Length = startNode.DistanceTo(this.EndNode);
        }

        public Node StartNode { get; set; }

        public Node EndNode { get; set; }

        public double Length { get; set; }

        public override string ToString()
        {
            return string.Format("{0} -> {1} : {2}", this.StartNode, this.EndNode, this.Length);
        }
    }
}
