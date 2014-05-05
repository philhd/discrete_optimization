using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravelingSalesman
{
    public interface ICooler
    {
        double GetNextTemperature(double currentTemp);
    }
}
