using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static alglib;

namespace MissionPlanner.Utilities
{
    public class VORStationsHandler
    {
        public List<VORStation> Stations { get; private set; }

        public VORStationsHandler()
        {

            Stations = new List<VORStation>
            {
                new VORStation("BKS", "BEKES VOR",         "464800N", "0210426E", 115.80),
                new VORStation("BUD", "BUDAPEST VOR",      "472702N", "0191458E", 117.30),
                new VORStation("BUG", "BUGAC VOR",         "464040N", "0194054E", 113.40),
                new VORStation("GYR", "GYOR VOR",          "473933N", "0174328E", 115.10),
                new VORStation("MNR", "MONOR VOR",         "472005N", "0192420E", 112.50),
                new VORStation("PTB", "PUSZTASZABOLCS VOR","470908N", "0184432E", 117.10),
                new VORStation("SAG", "SAJOHIDVEG VOR",    "480029N", "0205947E", 114.40),
                new VORStation("SVR", "SAGVAR VOR",        "464941N", "0180704E", 117.70),
                new VORStation("TPS", "TAPIOSAP VOR",      "472936N", "0192646E", 115.90)
            };

        }
    }
}
