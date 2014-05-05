using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class MoveDeciderSA : IMoveDecider
    {
        private Random rand = new Random(2);

        public MoveDeciderSA()
        {
            this.SimulateAnnealer = new SimulatedAnnealer(100);
        }

        public SimulatedAnnealer SimulateAnnealer { get; set; }

        public bool ShouldMakeMove(double prevDistance, double newDistance)
        {
            double delta = newDistance - prevDistance;

            if (delta < 0)
            {
                return true;
            }
            else
            {
                // no probability of accepting a worse neighbor
                double probTake = 0;

                if(this.SimulateAnnealer.Temperature > 0)
                {
                    probTake = Math.Exp((delta * -1) / this.SimulateAnnealer.Temperature);
                }

                double diceRoll = rand.NextDouble();

                if (diceRoll < probTake)
                {
                    return true;
                }

                this.SimulateAnnealer.UpdateTemperature();
            }

            return false;
        }
    }
}
