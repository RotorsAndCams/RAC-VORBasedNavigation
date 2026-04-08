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
            this.pnl_UP = new System.Windows.Forms.Panel();
            this.rtb_LogData = new System.Windows.Forms.RichTextBox();
            this.pnl_Down = new System.Windows.Forms.Panel();
            this.btn_SendExtPosToFC = new System.Windows.Forms.Button();
            this.btn_Error = new System.Windows.Forms.Button();
            this.btn_Filtering = new System.Windows.Forms.Button();
            this.btn_SetArduParametersTOExtNAV = new System.Windows.Forms.Button();
            this.btn_HideBlueLines = new System.Windows.Forms.Button();
            this.tlp_Base.SuspendLayout();
            this.pnl_UP.SuspendLayout();
            this.pnl_Down.SuspendLayout();
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
            this.rtb_GPSData.Size = new System.Drawing.Size(929, 240);
            this.rtb_GPSData.TabIndex = 0;
            this.rtb_GPSData.Text = "Simulation started";
            this.rtb_GPSData.WordWrap = false;
            // 
            // tlp_Base
            // 
            this.tlp_Base.ColumnCount = 2;
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tlp_Base.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlp_Base.Controls.Add(this.pnl_UP, 1, 0);
            this.tlp_Base.Controls.Add(this.rtb_LogData, 0, 1);
            this.tlp_Base.Controls.Add(this.rtb_GPSData, 0, 0);
            this.tlp_Base.Controls.Add(this.pnl_Down, 1, 1);
            this.tlp_Base.Location = new System.Drawing.Point(0, 0);
            this.tlp_Base.Name = "tlp_Base";
            this.tlp_Base.RowCount = 2;
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Base.Size = new System.Drawing.Size(1169, 493);
            this.tlp_Base.TabIndex = 1;
            // 
            // pnl_UP
            // 
            this.pnl_UP.Controls.Add(this.btn_Error);
            this.pnl_UP.Controls.Add(this.btn_Filtering);
            this.pnl_UP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_UP.Location = new System.Drawing.Point(938, 3);
            this.pnl_UP.Name = "pnl_UP";
            this.pnl_UP.Size = new System.Drawing.Size(228, 240);
            this.pnl_UP.TabIndex = 2;
            // 
            // rtb_LogData
            // 
            this.rtb_LogData.BackColor = System.Drawing.Color.Black;
            this.rtb_LogData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_LogData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_LogData.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.rtb_LogData.ForeColor = System.Drawing.Color.Crimson;
            this.rtb_LogData.Location = new System.Drawing.Point(3, 249);
            this.rtb_LogData.Name = "rtb_LogData";
            this.rtb_LogData.ReadOnly = true;
            this.rtb_LogData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb_LogData.Size = new System.Drawing.Size(929, 241);
            this.rtb_LogData.TabIndex = 2;
            this.rtb_LogData.Text = "...";
            this.rtb_LogData.WordWrap = false;
            // 
            // pnl_Down
            // 
            this.pnl_Down.Controls.Add(this.btn_HideBlueLines);
            this.pnl_Down.Controls.Add(this.btn_SetArduParametersTOExtNAV);
            this.pnl_Down.Controls.Add(this.btn_SendExtPosToFC);
            this.pnl_Down.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_Down.Location = new System.Drawing.Point(938, 249);
            this.pnl_Down.Name = "pnl_Down";
            this.pnl_Down.Size = new System.Drawing.Size(228, 241);
            this.pnl_Down.TabIndex = 4;
            // 
            // btn_SendExtPosToFC
            // 
            this.btn_SendExtPosToFC.Location = new System.Drawing.Point(6, 161);
            this.btn_SendExtPosToFC.Name = "btn_SendExtPosToFC";
            this.btn_SendExtPosToFC.Size = new System.Drawing.Size(168, 77);
            this.btn_SendExtPosToFC.TabIndex = 2;
            this.btn_SendExtPosToFC.Text = "Send ext pos to FC";
            this.btn_SendExtPosToFC.UseVisualStyleBackColor = true;
            this.btn_SendExtPosToFC.Visible = false;
            this.btn_SendExtPosToFC.Click += new System.EventHandler(this.btn_SendExtPosToFC_Click);
            // 
            // btn_Error
            // 
            this.btn_Error.Location = new System.Drawing.Point(3, 9);
            this.btn_Error.Name = "btn_Error";
            this.btn_Error.Size = new System.Drawing.Size(171, 77);
            this.btn_Error.TabIndex = 2;
            this.btn_Error.Text = "Simulate VOR bearing error";
            this.btn_Error.UseVisualStyleBackColor = true;
            this.btn_Error.Click += new System.EventHandler(this.btn_Error_Click);
            // 
            // btn_Filtering
            // 
            this.btn_Filtering.Location = new System.Drawing.Point(3, 92);
            this.btn_Filtering.Name = "btn_Filtering";
            this.btn_Filtering.Size = new System.Drawing.Size(171, 77);
            this.btn_Filtering.TabIndex = 3;
            this.btn_Filtering.Text = "Enable Filtering";
            this.btn_Filtering.UseVisualStyleBackColor = true;
            this.btn_Filtering.Click += new System.EventHandler(this.btn_Filtering_Click);
            // 
            // btn_SetArduParametersTOExtNAV
            // 
            this.btn_SetArduParametersTOExtNAV.Location = new System.Drawing.Point(6, 78);
            this.btn_SetArduParametersTOExtNAV.Name = "btn_SetArduParametersTOExtNAV";
            this.btn_SetArduParametersTOExtNAV.Size = new System.Drawing.Size(168, 77);
            this.btn_SetArduParametersTOExtNAV.TabIndex = 2;
            this.btn_SetArduParametersTOExtNAV.Text = "Set parameters to ext pos";
            this.btn_SetArduParametersTOExtNAV.UseVisualStyleBackColor = true;
            this.btn_SetArduParametersTOExtNAV.Visible = false;
            this.btn_SetArduParametersTOExtNAV.Click += new System.EventHandler(this.btn_SetArduParametersTOExtNAV_Click);
            // 
            // btn_HideBlueLines
            // 
            this.btn_HideBlueLines.Location = new System.Drawing.Point(6, 3);
            this.btn_HideBlueLines.Name = "btn_HideBlueLines";
            this.btn_HideBlueLines.Size = new System.Drawing.Size(168, 77);
            this.btn_HideBlueLines.TabIndex = 3;
            this.btn_HideBlueLines.Text = "lines from VOR stations";
            this.btn_HideBlueLines.UseVisualStyleBackColor = true;
            this.btn_HideBlueLines.Click += new System.EventHandler(this.btn_HideBlueLines_Click);
            // 
            // VORDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1174, 498);
            this.Controls.Add(this.tlp_Base);
            this.Name = "VORDataForm";
            this.Text = "VORDataForm";
            this.tlp_Base.ResumeLayout(false);
            this.pnl_UP.ResumeLayout(false);
            this.pnl_Down.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtb_GPSData;
        private System.Windows.Forms.TableLayoutPanel tlp_Base;
        private System.Windows.Forms.RichTextBox rtb_LogData;
        private System.Windows.Forms.Button btn_Error;
        private System.Windows.Forms.Button btn_Filtering;
        private System.Windows.Forms.Panel pnl_Down;
        private System.Windows.Forms.Button btn_SendExtPosToFC;
        private System.Windows.Forms.Panel pnl_UP;
        private System.Windows.Forms.Button btn_SetArduParametersTOExtNAV;
        private System.Windows.Forms.Button btn_HideBlueLines;
    }
}