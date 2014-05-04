using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public interface IMoveDecider
    {
        bool ShouldMakeMove(double prevDistance, double newDistance);
    }
}
