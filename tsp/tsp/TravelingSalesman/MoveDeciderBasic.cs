using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class MoveDeciderBasic : IMoveDecider
    {
        public bool ShouldMakeMove(double prevDistance, double newDistance)
        {
            if (newDistance < prevDistance)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
