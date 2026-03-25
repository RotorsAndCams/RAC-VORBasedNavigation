using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Utilities
{
    public class VORSignal
    {

        public string StationId;
        public double RadialFrom;     // radiál FROM az állomástól mért
        public double? DmeDistanceNm; // opcionális
        public double SignalQuality;  // 0–100%

        public VORSignal(string id, double radial, double? dist, double quality)
        {
            StationId = id;
            RadialFrom = radial;
            DmeDistanceNm = dist;
            SignalQuality = quality;
        }

    }
}
