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
            this.Start = startNode;
            this.End = endNode;
            this.Length = this.Start.DistanceTo(this.End);
        }

        public Node Start { get; set; }

        public Node End { get; set; }

        public double Length { get; set; }

        // swap the start and end nodes of the edge
        public void ReverseDirection()
        {
            Node temp = this.Start;
            this.Start = this.End;
            this.End = temp;
        }

        public void RecalcLength()
        {
            this.Length = this.Start.DistanceTo(this.End);
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1} : {2}", this.Start, this.End, this.Length);
        }
    }
}
