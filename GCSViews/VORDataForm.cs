using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MissionPlanner.GCSViews
{
    public partial class VORDataForm : Form
    {
        public VORDataForm()
        {
            InitializeComponent();

            this.rtb_GPSData.Text = "";
            this.rtb_LogData.Text = "";

        }

        public void AppendGPSDataLine(string p_NewLine)
        {
            if (rtb_GPSData.InvokeRequired)
            {
                rtb_GPSData.Invoke(new Action<string>(AppendGPSDataLine), p_NewLine);
                return;
            }

            rtb_GPSData.AppendText(p_NewLine + Environment.NewLine);

            rtb_GPSData.SelectionStart = rtb_GPSData.Text.Length;
            rtb_GPSData.ScrollToCaret();
        }

        public void AppendLogDataLine(string p_NewLine)
        {
            if (rtb_LogData.InvokeRequired)
            {
                rtb_LogData.Invoke(new Action<string>(AppendLogDataLine), p_NewLine);
                return;
            }
             
            rtb_LogData.AppendText(p_NewLine + Environment.NewLine);
            rtb_LogData.AppendText("------------------------------" + Environment.NewLine);
            rtb_LogData.SelectionStart = rtb_LogData.Text.Length;
            rtb_LogData.ScrollToCaret();
        }

        private void btn_Error_Click(object sender, EventArgs e)
        {
            MainV2.instance.FlightData._VORNav.AddRandomErrorToBearing = !MainV2.instance.FlightData._VORNav.AddRandomErrorToBearing;
        }

        private void btn_Filtering_Click(object sender, EventArgs e)
        {
            MainV2.instance.FlightData._VORNav.UseFiltering = !MainV2.instance.FlightData._VORNav.UseFiltering;
        }

        private void btn_SendExtPosToFC_Click(object sender, EventArgs e)
        {
            MainV2.instance.FlightData._VORNav.SendExternalDataToFC = !MainV2.instance.FlightData._VORNav.SendExternalDataToFC;
        }

        private void btn_SetArduParametersTOExtNAV_Click(object sender, EventArgs e)
        {
            //MainV2.instance.FlightData._VORNav.
        }

        private void btn_HideBlueLines_Click(object sender, EventArgs e)
        {
            MainV2.instance.FlightData.DrawBlueLines = !MainV2.instance.FlightData.DrawBlueLines;
        }

        //int calcDotCount = 0;

        //public void Calculating()
        //{
        //    if (rtb_LogData.InvokeRequired)
        //    {
        //        rtb_LogData.Invoke(new Action(Calculating));
        //        return;
        //    }


        //    if (calcDotCount < 3)
        //    {
        //        rtb_LogData.AppendText(".");
        //        calcDotCount++;
        //    }
        //    else
        //    {
        //        rtb_LogData.AppendText(Environment.NewLine);
        //        calcDotCount = 0;
        //    }

        //    rtb_LogData.SelectionStart = rtb_LogData.Text.Length;
        //    rtb_LogData.ScrollToCaret();

        //}
    }
}
