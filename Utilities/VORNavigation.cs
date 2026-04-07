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
    //--home=47.497913,19.040236,100,90 in simulator command  47.500103, 19.201489
    //--home=47.500103,19.201489,100,90
    public class VORNavigation
    {
        private System.Timers.Timer _NavigationTimer;
        private VORStationsHandler _VORStationsHandler;

        public VORStationsHandler VORStationsHandler { get { return _VORStationsHandler; } }

        public List<VORStation> TwoClosestStation { get { return GetTwoNearestVORs(this.VORStationsHandler.Stations, MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng); } }

        private double _RealGPSLat;
        private double _RealGPSLon;
        private double _RealGPSAlt;

        //clamp - filter
        private float _LastRecivedLat;
        private float _LastRecivedLng;
        private float _LastRecivedAlt;

        //clamp filter
        public double prevDx { get; private set; }
        public double prevDy { get; private set; }

        public double Radial1 { get; private set; }
        public double Radial2 { get; private set; }

        //filter
        private double fx, fy;
        private bool init = false;

        public float CalculatedLat { get; private set; }
        public float CalculatedLon { get; private set; }
        public float Z_Calculated { get; private set; }
        public float Yaw_Calculated { get; private set; }

        VORDataForm _dataForm;

        public bool AddRandomErrorToBearing { get; set; }
        public bool UseFiltering { get; set; }
        public bool SendExternalDataToFC {  get; set; }
        public bool ParamsToExtPosSet {  get; set; }

        VORGPSMovingAvarage _movingAvarageLAT;
        VORGPSMovingAvarage _movingAvarageLNG;

        public VORNavigation()
        {
            _NavigationTimer = new System.Timers.Timer();
            _NavigationTimer.Interval = 1000;
            _NavigationTimer.Elapsed += _NavigationTimer_Elapsed;

            _LastRecivedAlt = MainV2.comPort.MAV.GuidedMode.z;
            _LastRecivedLat = (float)(MainV2.comPort.MAV.GuidedMode.x / 1e7);
            _LastRecivedLng = (float)(MainV2.comPort.MAV.GuidedMode.y / 1e7);

            _RealGPSLat = MainV2.comPort.MAV.cs.lat;
            _RealGPSLon = MainV2.comPort.MAV.cs.lng;
            _RealGPSAlt = MainV2.comPort.MAV.cs.alt;

            _VORStationsHandler = new VORStationsHandler();

            _dataForm = new VORDataForm();

            AddRandomErrorToBearing = false;
            UseFiltering = false;
            SendExternalDataToFC = false;
            ParamsToExtPosSet = false;

            _movingAvarageLAT = new VORGPSMovingAvarage(25);
            _movingAvarageLNG = new VORGPSMovingAvarage(25);
        }

        private void SendExternalPosition(float p_X, float p_Y, float p_Z, float p_Yaw)
        {
            var home = MainV2.comPort.MAV.cs.HomeLocation;

            double dx = (p_X - home.Lng) * 111320 * Math.Cos(home.Lat * Math.PI / 180.0);
            double dy = (p_Y - home.Lat) * 110540;
            double dz = -(p_Z - home.Alt);  

            Filter(ref dx, ref dy);

            double maxStep = 0.5; // max half m difference in one packet
            dx = Clamp(dx, prevDx - maxStep, prevDx + maxStep);
            dy = Clamp(dy, prevDy - maxStep, prevDy + maxStep);

            if (Math.Abs(dx - prevDx) > 5 || Math.Abs(dy - prevDy) > 5)
                return; // do not send bad calculation

            prevDx = dx;
            prevDy = dy;

            if(!SendExternalDataToFC)
                return;

            _dataForm.AppendGPSDataLine("SENDING: dx: " + p_X + " dy: " + p_Y + " dz: " + dz);

            MainV2.comPort.sendPacket(
                new MAVLink.mavlink_vision_position_estimate_t()
                {
                    usec = (ulong)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds * 1000,
                    x = (float)p_X,
                    y = (float)p_Y,
                    z = -(float)(MainV2.comPort.MAV.cs.alt - home.Alt),
                    roll = 0,
                    pitch = 0,
                    yaw = 0
                },
                MainV2.comPort.MAV.sysid,
                MainV2.comPort.MAV.compid
                );
        }

        public static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        void Filter(ref double x, ref double y)
        {
            const double a = 0.02;  // 5% új adat, 95% simítás

            if (!init)
            {
                fx = x;
                fy = y;
                init = true;
            }

            fx = fx * (1 - a) + x * a;
            fy = fy * (1 - a) + y * a;

            x = fx;
            y = fy;
        }

        /// <summary>
        /// 20-30Hz-n küldje a számított pozíciót eredetileg, de már nem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _NavigationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CalculatePosition();
            SendExternalPosition(CalculatedLat, CalculatedLon, Z_Calculated, MainV2.comPort.MAV.cs.yaw);
            _dataForm.AppendGPSDataLine("External pos: lat: " + CalculatedLat + " ; lng: " + CalculatedLon);

            if (RadialChanged != null)
                RadialChanged(this, EventArgs.Empty);
        }


        public event EventHandler RadialChanged;

        //random error generator
        private Random rnd = new Random();

        private double RandomBearingError(double maxError = 1.0)
        {
            return (rnd.NextDouble() * 2.0 * maxError) - maxError;
        }


        private void CalculatePosition()
        {
            var vor1 = TwoClosestStation[0];
            var vor2 = TwoClosestStation[1];

            double radial1 = BearingFromAToB(MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng, vor1.LatitudeWgs84, vor1.LongitudeWgs84);
            double radial2 = BearingFromAToB(MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng, vor2.LatitudeWgs84, vor2.LongitudeWgs84);

            Radial1 = radial1;
            Radial2 = radial2;

            if (AddRandomErrorToBearing)
            {
                Radial1 += RandomBearingError();
                Radial2 += RandomBearingError();
            }

            _dataForm.AppendLogDataLine("Measured radial 1: " + (((Radial1 + 180.0) + 720) % 360) + " Measured radial 2: " + (((Radial2 + 180.0) + 720 ) % 360));

            double lat,lon;

            //hiányzik a hibakezelés ha nem számolta ki mert nem sikerült akkor inkább maradjon az előző
            BearingIntersectionSpherical(vor1.LatitudeWgs84, vor1.LongitudeWgs84, Radial1, vor2.LatitudeWgs84, vor2.LongitudeWgs84, Radial2, out lat, out lon);

            // simított érték:
            double smoothLat = _movingAvarageLAT.Update(lat);
            double smoothLon = _movingAvarageLNG.Update(lon);

            if (UseFiltering)
            {
                CalculatedLat = (float)smoothLat;
                CalculatedLon = (float)smoothLon;
            }
            else
            {
                CalculatedLat = (float)lat;
                CalculatedLon = (float)lon;
            }


            _dataForm.AppendLogDataLine("Calculated LAT: " + CalculatedLat + " Calculated LNG: " + CalculatedLon);

            //write error from real and calculated pos
            var dist = DistanceMeters(CalculatedLat, CalculatedLon, MainV2.comPort.MAV.cs.lat, MainV2.comPort.MAV.cs.lng);
            _dataForm.AppendLogDataLine("Distance error in meters: " + dist);
        }

        public void SendToHome()
        {
            MainV2.comPort.setMode("RTL");
        }

        public void SetArduParametersForVORNav()
        {
            //turn off gps
            MainV2.comPort.setParam("GPS_TYPE", 0);
            MainV2.comPort.setParam("AHRS_GPS_USE", 0);

            //external source x-y - EKFToExternalSource
            MainV2.comPort.setParam("EK3_SRC1_POSXY", 6);
            MainV2.comPort.setParam("EK3_SRC1_POSZ", 1);

            //EKFToExternalSource
            MainV2.comPort.setParam("EK3_SRC1_VELXY", 0);
            MainV2.comPort.setParam("EK3_SRC1_VELZ", 0);
            //vision odometry
            MainV2.comPort.setParam("VISO_TYPE", 1);

            //ekf tolerance
            MainV2.comPort.setParam("EK3_NOAID_THD ", 200);
            MainV2.comPort.setParam("EK3_POSNE_M_NSE", 5.0);
            MainV2.comPort.setParam("EK3_POSNE_E_NSE", 5.0);

            MainV2.comPort.setParam("EK3_NOAID_THD", 900);
            MainV2.comPort.setParam("EK3_GLITCH_RAD", 100);
            MainV2.comPort.setParam("EK3_GLITCH_ACCEL", 100);

            //forget viso
            MainV2.comPort.setParam("VISO_TYPE", 0);

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

        public static List<VORStation> GetTwoNearestVORs(List<VORStation> stations, double myLat, double myLon)
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

        public static double CalculateVORRadial(double vorLat, double vorLon, double aircraftLat, double aircraftLon)
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

        public static double CalculateAzimuth(double myLat, double myLon, double vorLat, double vorLon)
        {
            double distance, azi1, azi2;

            Geodesic.WGS84.Inverse(myLat, myLon, vorLat, vorLon,
                                   out distance, out azi1, out azi2);

            if (azi1 < 0) azi1 += 360.0;

            return azi1;
        }

        public static double BearingFromAToB(double lat1, double lon1, double lat2, double lon2)
        {
            double φ1 = lat1 * Math.PI / 180.0;
            double φ2 = lat2 * Math.PI / 180.0;
            double Δλ = (lon2 - lon1) * Math.PI / 180.0;

            double y = Math.Sin(Δλ) * Math.Cos(φ2);
            double x = Math.Cos(φ1) * Math.Sin(φ2) -
                       Math.Sin(φ1) * Math.Cos(φ2) * Math.Cos(Δλ);

            double θ = Math.Atan2(y, x);
            double bearing = (θ * 180.0 / Math.PI + 360.0) % 360.0;

            return bearing;
        }

        #endregion


        double DegToRad(double d) => d * Math.PI / 180.0;
        double RadToDeg(double r) => r * 180.0 / Math.PI;

        double[] LatLonToVector(double lat, double lon)
        {
            double φ = DegToRad(lat);
            double λ = DegToRad(lon);
            return new[]
            {
        Math.Cos(φ) * Math.Cos(λ),
        Math.Cos(φ) * Math.Sin(λ),
        Math.Sin(φ)
    };
        }

        double[] Cross(double[] a, double[] b)
        {
            return new[]
            {
        a[1]*b[2] - a[2]*b[1],
        a[2]*b[0] - a[0]*b[2],
        a[0]*b[1] - a[1]*b[0]
    };
        }

        double[] Normalize(double[] v)
        {
            double d = Math.Sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
            return new[] { v[0] / d, v[1] / d, v[2] / d };
        }

        double[] GreatCircleNormal(double lat, double lon, double bearingDeg)
        {
            double φ = DegToRad(lat);
            double λ = DegToRad(lon);
            double θ = DegToRad(bearingDeg);

            double[] north = { -Math.Sin(φ) * Math.Cos(λ), -Math.Sin(φ) * Math.Sin(λ), Math.Cos(φ) };
            double[] east = { -Math.Sin(λ), Math.Cos(λ), 0 };

            double[] direction =
            {
        east[0]*Math.Sin(θ) + north[0]*Math.Cos(θ),
        east[1]*Math.Sin(θ) + north[1]*Math.Cos(θ),
        east[2]*Math.Sin(θ) + north[2]*Math.Cos(θ)
    };

            var p = LatLonToVector(lat, lon);

            return Cross(p, direction);
        }

        public void BearingIntersectionSpherical(
            double lat1, double lon1, double bearing1,
            double lat2, double lon2, double bearing2,
            out double outLat, out double outLon)
        {
            var n1 = GreatCircleNormal(lat1, lon1, bearing1);
            var n2 = GreatCircleNormal(lat2, lon2, bearing2);

            var p = Cross(n1, n2);
            var pnorm = Normalize(p);

            outLat = RadToDeg(Math.Asin(pnorm[2]));
            outLon = RadToDeg(Math.Atan2(pnorm[1], pnorm[0]));
        }


    }
}
