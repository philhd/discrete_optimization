using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public class SimulatedAnnealer
    {
        public SimulatedAnnealer(double initialTemperature)
        {
            this.Temperature = initialTemperature;
            this.CoolingFunction = new CoolerLinear(.1);
        }

        public double Temperature { get; set; }

        public ICooler CoolingFunction { get; set; }

        public void UpdateTemperature()
        {
            this.Temperature = this.CoolingFunction.GetNextTemperature(this.Temperature);
        }
    }
}
