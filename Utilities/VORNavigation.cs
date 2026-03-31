using BruTile.Wms;
using GeographicLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IronPython.Modules._ast;
using GeographicLib;
using System.Windows.Forms;
using MissionPlanner.GCSViews;
using NetTopologySuite.Utilities;

namespace MissionPlanner.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class VORNavigation
    {
        private System.Timers.Timer _NavigationTimer;
        private VORStationsHandler _VORStationsHandler;

        public VORStationsHandler VORStationsHandler { get { return _VORStationsHandler; } }

        public List<VORStation> TwoClosestStation { get { return GetTwoNearestVORs(this.VORStationsHandler.Stations, MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng); } }


        public VORNavigation()
        {
            _NavigationTimer = new System.Timers.Timer();
            _NavigationTimer.Interval = 1000;
            _NavigationTimer.Elapsed += _NavigationTimer_Elapsed;

            _LastRecivedAlt = MainV2.comPort.MAV.GuidedMode.z;
            _LastRecivedLat = (float)(MainV2.comPort.MAV.GuidedMode.x / 1e7);
            _LastRecivedLng = (float)(MainV2.comPort.MAV.GuidedMode.y / 1e7);

            // ha letiltom a gps-t ezen még kéne tudonom a szimulált gps pozícióját
            _RealGPSLat = MainV2.comPort.MAV.cs.lat;
            _RealGPSLon = MainV2.comPort.MAV.cs.lng;
            _RealGPSAlt = MainV2.comPort.MAV.cs.alt;

            _VORStationsHandler = new VORStationsHandler();

            _dataForm = new VORDataForm();



        }

        private double _RealGPSLat;
        private double _RealGPSLon;
        private double _RealGPSAlt;

        private float _LastRecivedLat;
        private float _LastRecivedLng;
        private float _LastRecivedAlt;

        private void SendExternalPosition(float p_X, float p_Y, float p_Z, float p_Yaw)
        {

            // A VOR-ból számolt lat/lon WGS84 → helyi NED-re kell konvertálni!
            // Ez fontos! Az EKF helyi koordinátát vár, nem lat/lon-t.

            var home = MainV2.comPort.MAV.cs.HomeLocation;

            // Latitude/Longitude difference in meter
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

        public float CalculatedLat { get; private set; }
        public float CalculatedLon { get; private set; }
        public float Z_Calculated { get; private set; }
        public float Yaw_Calculated { get; private set; }

        VORDataForm _dataForm;

        

        /// <summary>
        /// 20-30Hz-n küldje a számított pozíciót eredetileg, de már nem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _NavigationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CalculatePosition();
            //SendExternalPosition(CalculatedLat, CalculatedLon, Z_Calculated, Yaw_Calculated);
            _dataForm.AppendGPSDataLine("External pos: lat: " + CalculatedLat + " ; lng: " + CalculatedLon);
        }

        private void CalculatePosition()
        {
            //exception handling???
            var vor1 = TwoClosestStation[0];
            var vor2 = TwoClosestStation[1];

            _dataForm.AppendLogDataLine("vor1: " + vor1.Name + " vor2" + vor2.Name);

            double radial1 = CalculateVORRadial(vor1.LatitudeWgs84, vor1.LongitudeWgs84, MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng);
            double radial2 = CalculateVORRadial(vor2.LatitudeWgs84, vor2.LongitudeWgs84, MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng);

            _dataForm.AppendLogDataLine("radial1: " + radial1 + " radial2: " + radial2);

            double lat,lon;


            bool ok = VorIntersection(
                vor1.LatitudeWgs84, vor1.LongitudeWgs84, radial1,
                vor2.LatitudeWgs84, vor2.LongitudeWgs84, radial2,
                out lat, out lon
                );

            if( ok )
            {
                CalculatedLat = (float)lat;
                CalculatedLon = (float)lon;
            }

            _dataForm.AppendLogDataLine("calculated LAT: " + CalculatedLat + " calculated LNG: " + CalculatedLon);
        }

        public void SendToHome()
        {
            MainV2.comPort.setMode("RTL");
        }

        public void TurnOffGPS()
        {
            MainV2.comPort.setParam("GPS_TYPE", 0);
            MainV2.comPort.setParam("AHRS_GPS_USE", 0);
        }

        

        public void EKFToExternalSource()
        {
            MainV2.comPort.setParam("EK3_SRC1_POSXY", 6);
            MainV2.comPort.setParam("EK3_SRC1_POSZ", 6);

            MainV2.comPort.setParam("EK3_SRC1_VELXY", 0);
            MainV2.comPort.setParam("EK3_SRC1_VELZ", 0);
        }

        public void EKFToOriginalSource()
        {
            //todo
        }

        public void TurnOnGPS()
        {
            MainV2.comPort.setParam("GPS_TYPE", 1);
            MainV2.comPort.setParam("AHRS_GPS_USE", 1);
        }


        public void StartFeedPosition()
        {
            _dataForm.Show();
            _dataForm.AppendLogDataLine("VOR simulation started");
            //_dataForm.Calculating();
            _NavigationTimer.Start();
        }

        public void StopFeedPosition()
        {

            _NavigationTimer.Stop();

        }


        #region VOR calculator methods

        public static double DistanceMeters(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371000; // Earth radius in meters
            double dLat = (lat2 - lat1) * Math.PI / 180.0;
            double dLon = (lon2 - lon1) * Math.PI / 180.0;

            lat1 = lat1 * Math.PI / 180.0;
            lat2 = lat2 * Math.PI / 180.0;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(lat1) * Math.Cos(lat2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }


        public static List<VORStation> GetTwoNearestVORs(
            List<VORStation> stations,
            double myLat,
            double myLon)
        {
            return stations
                .Select(st => new {
                    Station = st,
                    Dist = DistanceMeters(myLat, myLon, st.LatitudeWgs84, st.LongitudeWgs84)
                })
                .OrderBy(x => x.Dist)
                .Take(2)
                .Select(x => x.Station)
                .ToList();
        }

        

        public static double CalculateVORRadial(
    double vorLat, double vorLon,
    double aircraftLat, double aircraftLon)
    {
        double distance, azi1, azi2;

        // Inverse geodesic: VOR → plane
        Geodesic.WGS84.Inverse(vorLat, vorLon, aircraftLat, aircraftLon,
                               out distance, out azi1, out azi2);

        // VOR "bearing TO" would get from plane:
        // • azi1 = bearing FROM VOR TO airplane
        // the radial is the inverse:
        double radial = (azi1 + 180.0) % 360.0;

        if (radial < 0) radial += 360.0;

        return radial;
        }


        public static bool VorIntersection(
    double vor1Lat, double vor1Lon, double radial1,
    double vor2Lat, double vor2Lon, double radial2,
    out double outLat, out double outLon)
        {
            var geod = Geodesic.WGS84;

            // radial FROM → bearing TO
            double brg1 = (radial1 + 180.0) % 360.0;
            double brg2 = (radial2 + 180.0) % 360.0;

            // geodesic lines
            var line1 = geod.Line(vor1Lat, vor1Lon, brg1);
            var line2 = geod.Line(vor2Lat, vor2Lon, brg2);

            // iteration: two radius intercept
            double s1 = 0;
            double s2 = 0;
            const double step = 500; // 500 m step
            const double limit = 200000; // 200 km max

            for (int i = 0; i < (limit / step); i++)
            {
                // VOR1 radius point
                var p1 = line1.Position(s1);
                // VOR2 radius point
                var p2 = line2.Position(s2);

                // distance of two point
                double dist = geod.Inverse(p1.Latitude, p1.Longitude, p2.Latitude, p2.Longitude, out _, out _, out _);

                // close enough
                if (dist < 50) // 50 m
                {
                    outLat = (p1.Latitude + p2.Latitude) / 2.0;
                    outLon = (p1.Latitude + p2.Longitude) / 2.0;
                    return true;
                }

                // step throught in both radius
                s1 += step;
                s2 += step;
            }

            outLat = 0;
            outLon = 0;
            return false; // no intersect
        }

    #endregion

}
}
