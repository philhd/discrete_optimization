using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    class CoolerLinear : ICooler
    {
        public CoolerLinear(double initialRate)
        {
            this.LinearRate = initialRate;
        }

        public double LinearRate { get; set; }

        public double GetNextTemperature(double currentTemp)
        {
            double nextTemp = currentTemp - this.LinearRate;
            if (nextTemp > 0)
            {
                return nextTemp;
            }
            else
            {
                return 0;
            }
        }
    }
}
