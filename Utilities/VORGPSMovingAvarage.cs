using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Utilities
{
    public class VORGPSMovingAvarage
    {

        private readonly Queue<double> values = new Queue<double>();
        private readonly int windowSize;

        public VORGPSMovingAvarage(int windowSize = 10)
        {
            this.windowSize = windowSize;
        }

        public double Update(double input)
        {
            values.Enqueue(input);

            if (values.Count > windowSize)
                values.Dequeue();

            return values.Average();
        }

    }
}
