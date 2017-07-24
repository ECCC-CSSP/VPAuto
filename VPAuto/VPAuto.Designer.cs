namespace VPAuto
{
    partial class VPAuto
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
            this.components = new System.ComponentModel.Container();
            this.richTextBoxStatus = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.panelTop = new System.Windows.Forms.Panel();
            this.butStartVP = new System.Windows.Forms.Button();
            this.textBoxSaveSite = new System.Windows.Forms.TextBox();
            this.textBoxFromSite = new System.Windows.Forms.TextBox();
            this.lblFromSite = new System.Windows.Forms.Label();
            this.lblAcuteMixZoneValue = new System.Windows.Forms.Label();
            this.lblAcuteMixZone = new System.Windows.Forms.Label();
            this.lblPortSpacingValue = new System.Windows.Forms.Label();
            this.lblPortSpacing = new System.Windows.Forms.Label();
            this.lblNumberOfPortsValue = new System.Windows.Forms.Label();
            this.lblNumberOfPorts = new System.Windows.Forms.Label();
            this.lblHorizontalAngleValue = new System.Windows.Forms.Label();
            this.lblHorizontalAngle = new System.Windows.Forms.Label();
            this.lblVerticalAngleValue = new System.Windows.Forms.Label();
            this.lblVerticalAngle = new System.Windows.Forms.Label();
            this.lblPortElevationValue = new System.Windows.Forms.Label();
            this.lblPortElevation = new System.Windows.Forms.Label();
            this.lblPortDiameterValue = new System.Windows.Forms.Label();
            this.lblPortDiameter = new System.Windows.Forms.Label();
            this.lblVPScenarioIDValue = new System.Windows.Forms.Label();
            this.lblVPScenarioID = new System.Windows.Forms.Label();
            this.processPlumes = new System.Diagnostics.Process();
            this.timerCheckForDialogBoxToClose = new System.Windows.Forms.Timer(this.components);
            this.timerStopExecutionAfterOneSecond = new System.Windows.Forms.Timer(this.components);
            this.timerCheckDB = new System.Windows.Forms.Timer(this.components);
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBoxStatus
            // 
            this.richTextBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxStatus.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxStatus.Location = new System.Drawing.Point(0, 288);
            this.richTextBoxStatus.Name = "richTextBoxStatus";
            this.richTextBoxStatus.Size = new System.Drawing.Size(849, 115);
            this.richTextBoxStatus.TabIndex = 83;
            this.richTextBoxStatus.Text = "";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(546, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(254, 23);
            this.button2.TabIndex = 12;
            this.button2.Text = "Copy code to WMON01DTCHLEBL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.butStartVP);
            this.panelTop.Controls.Add(this.textBoxSaveSite);
            this.panelTop.Controls.Add(this.textBoxFromSite);
            this.panelTop.Controls.Add(this.lblFromSite);
            this.panelTop.Controls.Add(this.lblAcuteMixZoneValue);
            this.panelTop.Controls.Add(this.lblAcuteMixZone);
            this.panelTop.Controls.Add(this.lblPortSpacingValue);
            this.panelTop.Controls.Add(this.lblPortSpacing);
            this.panelTop.Controls.Add(this.lblNumberOfPortsValue);
            this.panelTop.Controls.Add(this.lblNumberOfPorts);
            this.panelTop.Controls.Add(this.lblHorizontalAngleValue);
            this.panelTop.Controls.Add(this.lblHorizontalAngle);
            this.panelTop.Controls.Add(this.lblVerticalAngleValue);
            this.panelTop.Controls.Add(this.lblVerticalAngle);
            this.panelTop.Controls.Add(this.lblPortElevationValue);
            this.panelTop.Controls.Add(this.lblPortElevation);
            this.panelTop.Controls.Add(this.lblPortDiameterValue);
            this.panelTop.Controls.Add(this.lblPortDiameter);
            this.panelTop.Controls.Add(this.lblVPScenarioIDValue);
            this.panelTop.Controls.Add(this.lblVPScenarioID);
            this.panelTop.Controls.Add(this.button2);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(849, 288);
            this.panelTop.TabIndex = 82;
            // 
            // butStartVP
            // 
            this.butStartVP.Location = new System.Drawing.Point(244, 6);
            this.butStartVP.Name = "butStartVP";
            this.butStartVP.Size = new System.Drawing.Size(75, 23);
            this.butStartVP.TabIndex = 32;
            this.butStartVP.Text = "Start VP";
            this.butStartVP.UseVisualStyleBackColor = true;
            this.butStartVP.Click += new System.EventHandler(this.butStartVP_Click);
            // 
            // textBoxSaveSite
            // 
            this.textBoxSaveSite.Location = new System.Drawing.Point(244, 53);
            this.textBoxSaveSite.Name = "textBoxSaveSite";
            this.textBoxSaveSite.Size = new System.Drawing.Size(556, 20);
            this.textBoxSaveSite.TabIndex = 31;
            this.textBoxSaveSite.Text = "http://wmon01dtchlebl2/csspwebtools/en-CA/VisualPlumes/SaveVPScenarioRawResultsJS" +
    "ON";
            // 
            // textBoxFromSite
            // 
            this.textBoxFromSite.Location = new System.Drawing.Point(244, 31);
            this.textBoxFromSite.Name = "textBoxFromSite";
            this.textBoxFromSite.Size = new System.Drawing.Size(556, 20);
            this.textBoxFromSite.TabIndex = 31;
            this.textBoxFromSite.Text = "http://wmon01dtchlebl2/csspwebtools/en-CA/VisualPlumes/GetNextVPScenarioToRunJSON" +
    "";
            // 
            // lblFromSite
            // 
            this.lblFromSite.AutoSize = true;
            this.lblFromSite.Location = new System.Drawing.Point(325, 15);
            this.lblFromSite.Name = "lblFromSite";
            this.lblFromSite.Size = new System.Drawing.Size(54, 13);
            this.lblFromSite.TabIndex = 30;
            this.lblFromSite.Text = "From Site:";
            // 
            // lblAcuteMixZoneValue
            // 
            this.lblAcuteMixZoneValue.AutoSize = true;
            this.lblAcuteMixZoneValue.Location = new System.Drawing.Point(92, 171);
            this.lblAcuteMixZoneValue.Name = "lblAcuteMixZoneValue";
            this.lblAcuteMixZoneValue.Size = new System.Drawing.Size(41, 13);
            this.lblAcuteMixZoneValue.TabIndex = 28;
            this.lblAcuteMixZoneValue.Text = "[empty]";
            // 
            // lblAcuteMixZone
            // 
            this.lblAcuteMixZone.AutoSize = true;
            this.lblAcuteMixZone.Location = new System.Drawing.Point(12, 171);
            this.lblAcuteMixZone.Name = "lblAcuteMixZone";
            this.lblAcuteMixZone.Size = new System.Drawing.Size(76, 13);
            this.lblAcuteMixZone.TabIndex = 27;
            this.lblAcuteMixZone.Text = "AcuteMixZone";
            // 
            // lblPortSpacingValue
            // 
            this.lblPortSpacingValue.AutoSize = true;
            this.lblPortSpacingValue.Location = new System.Drawing.Point(92, 146);
            this.lblPortSpacingValue.Name = "lblPortSpacingValue";
            this.lblPortSpacingValue.Size = new System.Drawing.Size(41, 13);
            this.lblPortSpacingValue.TabIndex = 26;
            this.lblPortSpacingValue.Text = "[empty]";
            // 
            // lblPortSpacing
            // 
            this.lblPortSpacing.AutoSize = true;
            this.lblPortSpacing.Location = new System.Drawing.Point(12, 146);
            this.lblPortSpacing.Name = "lblPortSpacing";
            this.lblPortSpacing.Size = new System.Drawing.Size(65, 13);
            this.lblPortSpacing.TabIndex = 25;
            this.lblPortSpacing.Text = "PortSpacing";
            // 
            // lblNumberOfPortsValue
            // 
            this.lblNumberOfPortsValue.AutoSize = true;
            this.lblNumberOfPortsValue.Location = new System.Drawing.Point(92, 124);
            this.lblNumberOfPortsValue.Name = "lblNumberOfPortsValue";
            this.lblNumberOfPortsValue.Size = new System.Drawing.Size(41, 13);
            this.lblNumberOfPortsValue.TabIndex = 24;
            this.lblNumberOfPortsValue.Text = "[empty]";
            // 
            // lblNumberOfPorts
            // 
            this.lblNumberOfPorts.AutoSize = true;
            this.lblNumberOfPorts.Location = new System.Drawing.Point(12, 124);
            this.lblNumberOfPorts.Name = "lblNumberOfPorts";
            this.lblNumberOfPorts.Size = new System.Drawing.Size(79, 13);
            this.lblNumberOfPorts.TabIndex = 23;
            this.lblNumberOfPorts.Text = "NumberOfPorts";
            // 
            // lblHorizontalAngleValue
            // 
            this.lblHorizontalAngleValue.AutoSize = true;
            this.lblHorizontalAngleValue.Location = new System.Drawing.Point(92, 102);
            this.lblHorizontalAngleValue.Name = "lblHorizontalAngleValue";
            this.lblHorizontalAngleValue.Size = new System.Drawing.Size(41, 13);
            this.lblHorizontalAngleValue.TabIndex = 22;
            this.lblHorizontalAngleValue.Text = "[empty]";
            // 
            // lblHorizontalAngle
            // 
            this.lblHorizontalAngle.AutoSize = true;
            this.lblHorizontalAngle.Location = new System.Drawing.Point(12, 102);
            this.lblHorizontalAngle.Name = "lblHorizontalAngle";
            this.lblHorizontalAngle.Size = new System.Drawing.Size(81, 13);
            this.lblHorizontalAngle.TabIndex = 21;
            this.lblHorizontalAngle.Text = "HorizontalAngle";
            // 
            // lblVerticalAngleValue
            // 
            this.lblVerticalAngleValue.AutoSize = true;
            this.lblVerticalAngleValue.Location = new System.Drawing.Point(92, 80);
            this.lblVerticalAngleValue.Name = "lblVerticalAngleValue";
            this.lblVerticalAngleValue.Size = new System.Drawing.Size(41, 13);
            this.lblVerticalAngleValue.TabIndex = 20;
            this.lblVerticalAngleValue.Text = "[empty]";
            // 
            // lblVerticalAngle
            // 
            this.lblVerticalAngle.AutoSize = true;
            this.lblVerticalAngle.Location = new System.Drawing.Point(12, 80);
            this.lblVerticalAngle.Name = "lblVerticalAngle";
            this.lblVerticalAngle.Size = new System.Drawing.Size(69, 13);
            this.lblVerticalAngle.TabIndex = 19;
            this.lblVerticalAngle.Text = "VerticalAngle";
            // 
            // lblPortElevationValue
            // 
            this.lblPortElevationValue.AutoSize = true;
            this.lblPortElevationValue.Location = new System.Drawing.Point(92, 56);
            this.lblPortElevationValue.Name = "lblPortElevationValue";
            this.lblPortElevationValue.Size = new System.Drawing.Size(41, 13);
            this.lblPortElevationValue.TabIndex = 18;
            this.lblPortElevationValue.Text = "[empty]";
            // 
            // lblPortElevation
            // 
            this.lblPortElevation.AutoSize = true;
            this.lblPortElevation.Location = new System.Drawing.Point(12, 56);
            this.lblPortElevation.Name = "lblPortElevation";
            this.lblPortElevation.Size = new System.Drawing.Size(70, 13);
            this.lblPortElevation.TabIndex = 17;
            this.lblPortElevation.Text = "PortElevation";
            // 
            // lblPortDiameterValue
            // 
            this.lblPortDiameterValue.AutoSize = true;
            this.lblPortDiameterValue.Location = new System.Drawing.Point(92, 34);
            this.lblPortDiameterValue.Name = "lblPortDiameterValue";
            this.lblPortDiameterValue.Size = new System.Drawing.Size(41, 13);
            this.lblPortDiameterValue.TabIndex = 16;
            this.lblPortDiameterValue.Text = "[empty]";
            // 
            // lblPortDiameter
            // 
            this.lblPortDiameter.AutoSize = true;
            this.lblPortDiameter.Location = new System.Drawing.Point(12, 34);
            this.lblPortDiameter.Name = "lblPortDiameter";
            this.lblPortDiameter.Size = new System.Drawing.Size(68, 13);
            this.lblPortDiameter.TabIndex = 15;
            this.lblPortDiameter.Text = "PortDiameter";
            // 
            // lblVPScenarioIDValue
            // 
            this.lblVPScenarioIDValue.AutoSize = true;
            this.lblVPScenarioIDValue.Location = new System.Drawing.Point(92, 11);
            this.lblVPScenarioIDValue.Name = "lblVPScenarioIDValue";
            this.lblVPScenarioIDValue.Size = new System.Drawing.Size(41, 13);
            this.lblVPScenarioIDValue.TabIndex = 14;
            this.lblVPScenarioIDValue.Text = "[empty]";
            // 
            // lblVPScenarioID
            // 
            this.lblVPScenarioID.AutoSize = true;
            this.lblVPScenarioID.Location = new System.Drawing.Point(12, 11);
            this.lblVPScenarioID.Name = "lblVPScenarioID";
            this.lblVPScenarioID.Size = new System.Drawing.Size(74, 13);
            this.lblVPScenarioID.TabIndex = 13;
            this.lblVPScenarioID.Text = "VPScenarioID";
            // 
            // processPlumes
            // 
            this.processPlumes.EnableRaisingEvents = true;
            this.processPlumes.StartInfo.Domain = "";
            this.processPlumes.StartInfo.LoadUserProfile = false;
            this.processPlumes.StartInfo.Password = null;
            this.processPlumes.StartInfo.StandardErrorEncoding = null;
            this.processPlumes.StartInfo.StandardOutputEncoding = null;
            this.processPlumes.StartInfo.UserName = "";
            this.processPlumes.SynchronizingObject = this;
            // 
            // timerCheckForDialogBoxToClose
            // 
            this.timerCheckForDialogBoxToClose.Interval = 1000;
            this.timerCheckForDialogBoxToClose.Tick += new System.EventHandler(this.timerCheckForDialogBoxToClose_Tick);
            // 
            // timerStopExecutionAfterOneSecond
            // 
            this.timerStopExecutionAfterOneSecond.Interval = 1000;
            this.timerStopExecutionAfterOneSecond.Tick += new System.EventHandler(this.timerStopExecutionAfterOneSecond_Tick);
            // 
            // timerCheckDB
            // 
            this.timerCheckDB.Interval = 5000;
            this.timerCheckDB.Tick += new System.EventHandler(this.timerCheckDB_Tick);
            // 
            // VPAuto
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(849, 403);
            this.Controls.Add(this.richTextBoxStatus);
            this.Controls.Add(this.panelTop);
            this.Name = "VPAuto";
            this.Text = "VP Auto Application";
            this.Load += new System.EventHandler(this.VPAuto_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxStatus;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panelTop;
        private System.Diagnostics.Process processPlumes;
        private System.Windows.Forms.Timer timerCheckForDialogBoxToClose;
        private System.Windows.Forms.Timer timerStopExecutionAfterOneSecond;
        private System.Windows.Forms.Timer timerCheckDB;
        private System.Windows.Forms.Label lblVPScenarioIDValue;
        private System.Windows.Forms.Label lblVPScenarioID;
        private System.Windows.Forms.Label lblAcuteMixZoneValue;
        private System.Windows.Forms.Label lblAcuteMixZone;
        private System.Windows.Forms.Label lblPortSpacingValue;
        private System.Windows.Forms.Label lblPortSpacing;
        private System.Windows.Forms.Label lblNumberOfPortsValue;
        private System.Windows.Forms.Label lblNumberOfPorts;
        private System.Windows.Forms.Label lblHorizontalAngleValue;
        private System.Windows.Forms.Label lblHorizontalAngle;
        private System.Windows.Forms.Label lblVerticalAngleValue;
        private System.Windows.Forms.Label lblVerticalAngle;
        private System.Windows.Forms.Label lblPortElevationValue;
        private System.Windows.Forms.Label lblPortElevation;
        private System.Windows.Forms.Label lblPortDiameterValue;
        private System.Windows.Forms.Label lblPortDiameter;
        private System.Windows.Forms.TextBox textBoxFromSite;
        private System.Windows.Forms.Label lblFromSite;
        private System.Windows.Forms.TextBox textBoxSaveSite;
        private System.Windows.Forms.Button butStartVP;
    }
}

