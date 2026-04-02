using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Utilities
{
    public class VORBearingSmooter
    {

        private double angle;
        private bool initialized = false;

        public double Update(double newAngleDeg, double alpha = 0.1)
        {
            double oldRad, newRad;

            if (!initialized)
            {
                angle = newAngleDeg;
                initialized = true;
                return angle;
            }

            oldRad = angle * Math.PI / 180.0;
            newRad = newAngleDeg * Math.PI / 180.0;

            // vector average
            double x = Math.Cos(oldRad) * (1 - alpha) + Math.Cos(newRad) * alpha;
            double y = Math.Sin(oldRad) * (1 - alpha) + Math.Sin(newRad) * alpha;

            angle = Math.Atan2(y, x) * 180.0 / Math.PI;
            if (angle < 0) angle += 360.0;

            return angle;
        }

    }
}
