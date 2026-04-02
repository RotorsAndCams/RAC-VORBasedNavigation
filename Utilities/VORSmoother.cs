using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Utilities
{
    public class VORSmoother
    {

        private double value;
        private bool initialized = false;

        public double Update(double input, double alpha = 0.1)
        {
            if (!initialized)
            {
                value = input;
                initialized = true;
                return value;
            }

            value = value * (1 - alpha) + input * alpha;
            return value;
        }

    }
}
