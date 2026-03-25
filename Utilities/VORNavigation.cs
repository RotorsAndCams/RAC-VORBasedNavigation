using BruTile.Wms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;

namespace MissionPlanner.Utilities
{
    public class VORNavigation
    {
        private System.Timers.Timer _NavigationTimer;

        public VORNavigation()
        {
            _NavigationTimer = new System.Timers.Timer();
            _NavigationTimer.Interval = 50;
            _NavigationTimer.Elapsed += _NavigationTimer_Elapsed;



        }

        private void SendExternalPosition(float p_X, float p_Y, float p_Z, float p_Yaw)
        {

            // A VOR-ból számolt lat/lon WGS84 → helyi NED-re kell konvertálni!
            // Ez fontos! Az EKF helyi koordinátát vár, nem lat/lon-t.

            var home = MainV2.comPort.MAV.cs.HomeLocation;

            // Latitude/Longitude különbségből méter
            double dx = (p_X - home.Lng) * 111320 * Math.Cos(home.Lat * Math.PI / 180.0);
            double dy = (p_Y - home.Lat) * 110540;
            double dz = -p_Z;  // NED: lefelé pozitív (ArduPilot így használja)


            MainV2.comPort.sendPacket(
                new MAVLink.mavlink_vision_position_estimate_t()
                {
                    usec = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds * 1000,
                    x = (float)dx,     // vagy lat hosszabbítás
                    y = (float)dy,
                    z = (float)dz,
                    roll = 0,
                    pitch = 0,
                    yaw = p_Yaw
                },
                MainV2.comPort.MAV.sysid,
                MainV2.comPort.MAV.compid
                );
        }

    //    public static void VORToLatLon(
    //double lat1, double lon1, double radial1,
    //double lat2, double lon2, double radial2,
    //out double latOut, out double lonOut)
    //    {
    //        // Radiánba
    //        double r1 = radial1 * Math.PI / 180.0;
    //        double r2 = radial2 * Math.PI / 180.0;

    //        // Egyszerűsített vonal-metszés földgömbön (közelítő, de nagyon jó)
    //        // Vonalak: VOR + bearing (radial + 180)
    //        var brg1 = (radial1 + 180.0) % 360.0;
    //        var brg2 = (radial2 + 180.0) % 360.0;

    //        // Használjunk GeographicLib-et pontos metszéshez:
    //        var g = new GeographicLib.Geodesic(GeographicLib.Geodesic.WGS84);

    //        GeographicLib.GeodesicLine line1 = g.Line(lat1, lon1, brg1);
    //        GeographicLib.GeodesicLine line2 = g.Line(lat2, lon2, brg2);

    //        GeographicLib.Geodesic.Intersect(
    //            ref line1, ref line2,
    //            out latOut, out lonOut
    //        );
    //    }

        private void _NavigationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {


            
            //SendExternalPosition();
        }

        public void SendToHome()
        {

        }

        public void TurnOffGPS()
        {
            MainV2.comPort.setParam("GPS_TYPE", 0);
            MainV2.comPort.setParam("AHRS_GPS_USE", 0);
        }

        public void TurnOnGPS()
        {
            MainV2.comPort.setParam("GPS_TYPE", 1);
            MainV2.comPort.setParam("AHRS_GPS_USE", 1);
        }

        public void EKFToExternalSource()
        {
            MainV2.comPort.setParam("EK3_SRC1_POSXY", 6);
            MainV2.comPort.setParam("EK3_SRC1_POSZ", 6);

            MainV2.comPort.setParam("EK3_SRC1_VELXY", 0);
            MainV2.comPort.setParam("EK3_SRC1_VELZ", 0);
        }



    }
}
