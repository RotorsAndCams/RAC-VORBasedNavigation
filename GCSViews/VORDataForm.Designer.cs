namespace MissionPlanner.GCSViews
{
    partial class VORDataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.rtb_GPSData = new System.Windows.Forms.RichTextBox();
            this.tlp_Base = new System.Windows.Forms.TableLayoutPanel();
            this.rtb_LogData = new System.Windows.Forms.RichTextBox();
            this.tlp_Base.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtb_GPSData
            // 
            this.rtb_GPSData.BackColor = System.Drawing.Color.Black;
            this.rtb_GPSData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_GPSData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_GPSData.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_GPSData.ForeColor = System.Drawing.Color.ForestGreen;
            this.rtb_GPSData.Location = new System.Drawing.Point(3, 3);
            this.rtb_GPSData.Name = "rtb_GPSData";
            this.rtb_GPSData.ReadOnly = true;
            this.rtb_GPSData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb_GPSData.Size = new System.Drawing.Size(609, 208);
            this.rtb_GPSData.TabIndex = 0;
            this.rtb_GPSData.Text = "Simulation started";
            this.rtb_GPSData.WordWrap = false;
            // 
            // tlp_Base
            // 
            this.tlp_Base.ColumnCount = 1;
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Controls.Add(this.rtb_LogData, 0, 1);
            this.tlp_Base.Controls.Add(this.rtb_GPSData, 0, 0);
            this.tlp_Base.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Base.Location = new System.Drawing.Point(0, 0);
            this.tlp_Base.Name = "tlp_Base";
            this.tlp_Base.RowCount = 2;
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Size = new System.Drawing.Size(615, 429);
            this.tlp_Base.TabIndex = 1;
            // 
            // rtb_LogData
            // 
            this.rtb_LogData.BackColor = System.Drawing.Color.Black;
            this.rtb_LogData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_LogData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_LogData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_LogData.ForeColor = System.Drawing.Color.Crimson;
            this.rtb_LogData.Location = new System.Drawing.Point(3, 217);
            this.rtb_LogData.Name = "rtb_LogData";
            this.rtb_LogData.ReadOnly = true;
            this.rtb_LogData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb_LogData.Size = new System.Drawing.Size(609, 209);
            this.rtb_LogData.TabIndex = 2;
            this.rtb_LogData.Text = "...";
            this.rtb_LogData.WordWrap = false;
            // 
            // VORDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(615, 429);
            this.Controls.Add(this.tlp_Base);
            this.Name = "VORDataForm";
            this.Text = "VORDataForm";
            this.tlp_Base.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_GPSData;
        private System.Windows.Forms.TableLayoutPanel tlp_Base;
        private System.Windows.Forms.RichTextBox rtb_LogData;
    }
}