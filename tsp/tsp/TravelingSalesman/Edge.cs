using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class Edge
    {
        private static int NextId = 0;

        public Edge(Node startNode, Node endNode)
        {
            this.Start = startNode;
            this.End = endNode;
            this.Length = this.Start.DistanceTo(this.End);
            this.Id = GetNextId();
        }

        public int Id { get; set; }

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

        private static int GetNextId()
        {
            return System.Threading.Interlocked.Increment(ref NextId);
        }
    }
}
