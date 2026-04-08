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
                new VORStation("BKS", "BEKES VOR",         "46.800000", "21.073888", 115.80),  //"464800N", "0210426E"
                new VORStation("BUD", "BUDAPEST VOR",      "47.450556", "19.249444", 117.30),  //"472702N", "0191458E"
                new VORStation("BUG", "BUGAC VOR",         "46.677778", "19.681667", 113.40),  //"464040N", "0194054E"
                new VORStation("GYR", "GYOR VOR",          "47.659167", "17.724445", 115.10),  //"473933N", "0174328E"
                new VORStation("MNR", "MONOR VOR",         "47.334722", "19.405556", 112.50),  //"472005N", "0192420E"
                new VORStation("PTB", "PUSZTASZABOLCS VOR","47.152222", "18.742222", 117.10),  //"470908N", "0184432E"
                new VORStation("SAG", "SAJOHIDVEG VOR",    "48.008056", "20.996389", 114.40),  //"480029N", "0205947E"
                new VORStation("SVR", "SAGVAR VOR",        "46.828056", "18.117778", 117.70),  //"464941N", "0180704E"
                new VORStation("TPS", "TAPIOSAP VOR",      "47.493333", "19.446111", 115.90),   //"472936N", "0192646E"
                new VORStation("NYR", "NYIREGYHAZA VOR",   "47.991199", "21.692600", 116.10)
            };

        }
    }
}
