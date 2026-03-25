using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeographicLib;
using DotSpatial.Projections.ProjectedCategories;

namespace MissionPlanner.Utilities
{
    public class VORStation
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string LatRaw { get; set; }   // Eredeti formátum pl. 464800N
        public string LonRaw { get; set; }   // Eredeti formátum pl. 0210426E
        public double Frequency { get; set; }

        // WGS84 decimális fokok
        public double LatitudeWgs84 { get; private set; }
        public double LongitudeWgs84 { get; private set; }

        public VORStation(string id, string name, string latRaw, string lonRaw, double freq)
        {
            Id = id;
            Name = name;
            LatRaw = latRaw;
            LonRaw = lonRaw;
            Frequency = freq;

            LatitudeWgs84 = DmsToDecimal(latRaw);
            LongitudeWgs84 = DmsToDecimal(lonRaw);

        }

        public static double DmsToDecimal(string dms)
        {
            if (string.IsNullOrEmpty(dms))
                throw new ArgumentException("DMS string is null or empty.");

            dms = dms.Trim().ToUpper();

            // Utolsó karakter: N, S, E vagy W
            char dir = dms[dms.Length - 1];

            // Töröljük az irány betűt
            string numbers = dms.Substring(0, dms.Length - 1);

            int deg = 0;
            int min = 0;
            int sec = 0;

            // Latitude → DDMMSS (6 számjegy)
            // Longitude → DDDMMSS (7 számjegy)
            if (numbers.Length == 6)
            {
                deg = int.Parse(numbers.Substring(0, 2));
                min = int.Parse(numbers.Substring(2, 2));
                sec = int.Parse(numbers.Substring(4, 2));
            }
            else if (numbers.Length == 7)
            {
                deg = int.Parse(numbers.Substring(0, 3));
                min = int.Parse(numbers.Substring(3, 2));
                sec = int.Parse(numbers.Substring(5, 2));
            }
            else
            {
                throw new FormatException("Invalid DMS coordinate format: " + dms);
            }

            double value = deg + (min / 60.0) + (sec / 3600.0);

            // Déli és nyugati irány = negatív
            if (dir == 'S' || dir == 'W')
                value = -value;

            return value;
        }

    }
}
