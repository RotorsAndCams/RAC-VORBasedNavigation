using GeographicLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionPlanner.Utilities
{
    /// <summary>
    /// VOR Signal Generator
    /// </summary>
    public class VORSimulator
    {
        static Geodesic geo = Geodesic.WGS84;

        public static VORSignal GenerateSignal(VORStation station, double aircraftLat, double aircraftLon, bool withDME = true)
        {
            double azi1, azi2, dist;

            // 
            // Geodéziai irány az állomástól a repülőig
            //
            geo.Inverse(station.LatitudeWgs84, station.LongitudeWgs84, aircraftLat, aircraftLon,
                        out dist, out azi1, out azi2);

            // Radiál = bearing FROM → az ellenkező irány
            double radialFrom = (azi2 + 180.0) % 360.0;

            // DME távolság tengeri mérföldben
            double dmeNm = withDME ? (dist / 1852.0) : double.NaN;

            // Jelminőség egyszerű modell
            double quality = Math.Max(0, 100 - (dist / 50000.0)); // 50 km felett romlik

            return new VORSignal(
                station.Id,
                radialFrom,
                withDME ? dmeNm : (double?)null,
                quality
            );
        }
    }
}
