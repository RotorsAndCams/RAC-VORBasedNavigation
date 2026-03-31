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
