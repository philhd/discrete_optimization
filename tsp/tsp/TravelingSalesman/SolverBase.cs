using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public abstract class SolverBase : ISolver
    {
        public event Action DataComplete;

        public TspGraph Graph { get; protected set; }

        public IMoveDecider MoveDecider { get; set; }

        public abstract TspGraph Solve(IEnumerable<Node> nodes);

        protected void RaiseDataComplete()
        {
            if (this.DataComplete != null)
            {
                this.DataComplete();
            }
        }
    }
}
