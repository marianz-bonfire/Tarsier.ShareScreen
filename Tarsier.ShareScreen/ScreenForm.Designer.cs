namespace Tarsier.ShareScreen
{
    partial class ScreenForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.lblSourceType = new System.Windows.Forms.Label();
            this.cboSourceType = new System.Windows.Forms.ComboBox();
            this.lblResolution = new System.Windows.Forms.Label();
            this.cboResolution = new System.Windows.Forms.ComboBox();
            this.lblFps = new System.Windows.Forms.Label();
            this.cboFps = new System.Windows.Forms.ComboBox();
            this.lblMonitor = new System.Windows.Forms.Label();
            this.cboMonitor = new System.Windows.Forms.ComboBox();
            this.btnRefreshMonitors = new System.Windows.Forms.Button();
            this.lblWindow = new System.Windows.Forms.Label();
            this.cboWindows = new System.Windows.Forms.ComboBox();
            this.btnRefreshWindows = new System.Windows.Forms.Button();
            this.lblWebcam = new System.Windows.Forms.Label();
            this.cboWebcam = new System.Windows.Forms.ComboBox();
            this.btnRefreshWebcams = new System.Windows.Forms.Button();
            this.chkShowCursor = new System.Windows.Forms.CheckBox();
            this.grpServer = new System.Windows.Forms.GroupBox();
            this.lblIpAddress = new System.Windows.Forms.Label();
            this.cboIpAddress = new System.Windows.Forms.ComboBox();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.grpStream = new System.Windows.Forms.GroupBox();
            this.lblStreamUrlCaption = new System.Windows.Forms.Label();
            this.txtStreamUrl = new System.Windows.Forms.TextBox();
            this.btnCopyUrl = new System.Windows.Forms.Button();
            this.btnOpenBrowser = new System.Windows.Forms.Button();
            this.lblStatusCaption = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblClientsCaption = new System.Windows.Forms.Label();
            this.lblClientCount = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.grpInstructions = new System.Windows.Forms.GroupBox();
            this.txtInstructions = new System.Windows.Forms.TextBox();
            this.tmrClientCount = new System.Windows.Forms.Timer();
            this.grpSource.SuspendLayout();
            this.grpServer.SuspendLayout();
            this.grpStream.SuspendLayout();
            this.grpInstructions.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpSource
            // 
            this.grpSource.Controls.Add(this.chkShowCursor);
            this.grpSource.Controls.Add(this.btnRefreshWebcams);
            this.grpSource.Controls.Add(this.cboWebcam);
            this.grpSource.Controls.Add(this.lblWebcam);
            this.grpSource.Controls.Add(this.btnRefreshWindows);
            this.grpSource.Controls.Add(this.cboWindows);
            this.grpSource.Controls.Add(this.lblWindow);
            this.grpSource.Controls.Add(this.btnRefreshMonitors);
            this.grpSource.Controls.Add(this.cboMonitor);
            this.grpSource.Controls.Add(this.lblMonitor);
            this.grpSource.Controls.Add(this.cboFps);
            this.grpSource.Controls.Add(this.lblFps);
            this.grpSource.Controls.Add(this.cboResolution);
            this.grpSource.Controls.Add(this.lblResolution);
            this.grpSource.Controls.Add(this.cboSourceType);
            this.grpSource.Controls.Add(this.lblSourceType);
            this.grpSource.Location = new System.Drawing.Point(12, 12);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(440, 260);
            this.grpSource.TabIndex = 0;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source Settings";
            // 
            // lblSourceType
            // 
            this.lblSourceType.AutoSize = true;
            this.lblSourceType.Location = new System.Drawing.Point(15, 28);
            this.lblSourceType.Name = "lblSourceType";
            this.lblSourceType.Size = new System.Drawing.Size(72, 13);
            this.lblSourceType.TabIndex = 0;
            this.lblSourceType.Text = "Source Type:";
            // 
            // cboSourceType
            // 
            this.cboSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSourceType.FormattingEnabled = true;
            this.cboSourceType.Location = new System.Drawing.Point(120, 25);
            this.cboSourceType.Name = "cboSourceType";
            this.cboSourceType.Size = new System.Drawing.Size(200, 21);
            this.cboSourceType.TabIndex = 1;
            this.cboSourceType.SelectedIndexChanged += new System.EventHandler(this.cboSourceType_SelectedIndexChanged);
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.Location = new System.Drawing.Point(15, 58);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(63, 13);
            this.lblResolution.TabIndex = 2;
            this.lblResolution.Text = "Resolution:";
            // 
            // cboResolution
            // 
            this.cboResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboResolution.FormattingEnabled = true;
            this.cboResolution.Location = new System.Drawing.Point(120, 55);
            this.cboResolution.Name = "cboResolution";
            this.cboResolution.Size = new System.Drawing.Size(200, 21);
            this.cboResolution.TabIndex = 3;
            // 
            // lblFps
            // 
            this.lblFps.AutoSize = true;
            this.lblFps.Location = new System.Drawing.Point(15, 88);
            this.lblFps.Name = "lblFps";
            this.lblFps.Size = new System.Drawing.Size(30, 13);
            this.lblFps.TabIndex = 4;
            this.lblFps.Text = "FPS:";
            // 
            // cboFps
            // 
            this.cboFps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFps.FormattingEnabled = true;
            this.cboFps.Location = new System.Drawing.Point(120, 85);
            this.cboFps.Name = "cboFps";
            this.cboFps.Size = new System.Drawing.Size(200, 21);
            this.cboFps.TabIndex = 5;
            // 
            // lblMonitor
            // 
            this.lblMonitor.AutoSize = true;
            this.lblMonitor.Location = new System.Drawing.Point(15, 118);
            this.lblMonitor.Name = "lblMonitor";
            this.lblMonitor.Size = new System.Drawing.Size(45, 13);
            this.lblMonitor.TabIndex = 6;
            this.lblMonitor.Text = "Monitor:";
            // 
            // cboMonitor
            // 
            this.cboMonitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMonitor.FormattingEnabled = true;
            this.cboMonitor.Location = new System.Drawing.Point(120, 115);
            this.cboMonitor.Name = "cboMonitor";
            this.cboMonitor.Size = new System.Drawing.Size(200, 21);
            this.cboMonitor.TabIndex = 7;
            // 
            // btnRefreshMonitors
            // 
            this.btnRefreshMonitors.Location = new System.Drawing.Point(330, 113);
            this.btnRefreshMonitors.Name = "btnRefreshMonitors";
            this.btnRefreshMonitors.Size = new System.Drawing.Size(95, 25);
            this.btnRefreshMonitors.TabIndex = 8;
            this.btnRefreshMonitors.Text = "Refresh";
            this.btnRefreshMonitors.UseVisualStyleBackColor = true;
            this.btnRefreshMonitors.Click += new System.EventHandler(this.btnRefreshMonitors_Click);
            // 
            // lblWindow
            // 
            this.lblWindow.AutoSize = true;
            this.lblWindow.Location = new System.Drawing.Point(15, 148);
            this.lblWindow.Name = "lblWindow";
            this.lblWindow.Size = new System.Drawing.Size(49, 13);
            this.lblWindow.TabIndex = 9;
            this.lblWindow.Text = "Window:";
            // 
            // cboWindows
            // 
            this.cboWindows.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWindows.FormattingEnabled = true;
            this.cboWindows.Location = new System.Drawing.Point(120, 145);
            this.cboWindows.Name = "cboWindows";
            this.cboWindows.Size = new System.Drawing.Size(200, 21);
            this.cboWindows.TabIndex = 10;
            // 
            // btnRefreshWindows
            // 
            this.btnRefreshWindows.Location = new System.Drawing.Point(330, 143);
            this.btnRefreshWindows.Name = "btnRefreshWindows";
            this.btnRefreshWindows.Size = new System.Drawing.Size(95, 25);
            this.btnRefreshWindows.TabIndex = 11;
            this.btnRefreshWindows.Text = "Refresh";
            this.btnRefreshWindows.UseVisualStyleBackColor = true;
            this.btnRefreshWindows.Click += new System.EventHandler(this.btnRefreshWindows_Click);
            // 
            // lblWebcam
            // 
            this.lblWebcam.AutoSize = true;
            this.lblWebcam.Location = new System.Drawing.Point(15, 178);
            this.lblWebcam.Name = "lblWebcam";
            this.lblWebcam.Size = new System.Drawing.Size(52, 13);
            this.lblWebcam.TabIndex = 12;
            this.lblWebcam.Text = "Webcam:";
            // 
            // cboWebcam
            // 
            this.cboWebcam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWebcam.FormattingEnabled = true;
            this.cboWebcam.Location = new System.Drawing.Point(120, 175);
            this.cboWebcam.Name = "cboWebcam";
            this.cboWebcam.Size = new System.Drawing.Size(200, 21);
            this.cboWebcam.TabIndex = 13;
            // 
            // btnRefreshWebcams
            // 
            this.btnRefreshWebcams.Location = new System.Drawing.Point(330, 173);
            this.btnRefreshWebcams.Name = "btnRefreshWebcams";
            this.btnRefreshWebcams.Size = new System.Drawing.Size(95, 25);
            this.btnRefreshWebcams.TabIndex = 14;
            this.btnRefreshWebcams.Text = "Refresh";
            this.btnRefreshWebcams.UseVisualStyleBackColor = true;
            this.btnRefreshWebcams.Click += new System.EventHandler(this.btnRefreshWebcams_Click);
            // 
            // chkShowCursor
            // 
            this.chkShowCursor.AutoSize = true;
            this.chkShowCursor.Checked = true;
            this.chkShowCursor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkShowCursor.Location = new System.Drawing.Point(120, 208);
            this.chkShowCursor.Name = "chkShowCursor";
            this.chkShowCursor.Size = new System.Drawing.Size(131, 17);
            this.chkShowCursor.TabIndex = 15;
            this.chkShowCursor.Text = "Show Mouse Cursor";
            this.chkShowCursor.UseVisualStyleBackColor = true;
            // 
            // grpServer
            // 
            this.grpServer.Controls.Add(this.txtPort);
            this.grpServer.Controls.Add(this.lblPort);
            this.grpServer.Controls.Add(this.cboIpAddress);
            this.grpServer.Controls.Add(this.lblIpAddress);
            this.grpServer.Location = new System.Drawing.Point(12, 280);
            this.grpServer.Name = "grpServer";
            this.grpServer.Size = new System.Drawing.Size(440, 60);
            this.grpServer.TabIndex = 1;
            this.grpServer.TabStop = false;
            this.grpServer.Text = "Server Settings";
            // 
            // lblIpAddress
            // 
            this.lblIpAddress.AutoSize = true;
            this.lblIpAddress.Location = new System.Drawing.Point(15, 28);
            this.lblIpAddress.Name = "lblIpAddress";
            this.lblIpAddress.Size = new System.Drawing.Size(64, 13);
            this.lblIpAddress.TabIndex = 0;
            this.lblIpAddress.Text = "IP Address:";
            // 
            // cboIpAddress
            // 
            this.cboIpAddress.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIpAddress.FormattingEnabled = true;
            this.cboIpAddress.Location = new System.Drawing.Point(120, 25);
            this.cboIpAddress.Name = "cboIpAddress";
            this.cboIpAddress.Size = new System.Drawing.Size(200, 21);
            this.cboIpAddress.TabIndex = 1;
            this.cboIpAddress.SelectedIndexChanged += new System.EventHandler(this.cboIpAddress_SelectedIndexChanged);
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(330, 28);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(29, 13);
            this.lblPort.TabIndex = 2;
            this.lblPort.Text = "Port:";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(365, 25);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(60, 20);
            this.txtPort.TabIndex = 3;
            this.txtPort.Text = "9000";
            this.txtPort.TextChanged += new System.EventHandler(this.txtPort_TextChanged);
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(12, 350);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(215, 35);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "▶  Start Streaming";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.btnStop.Location = new System.Drawing.Point(237, 350);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(215, 35);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "■  Stop Streaming";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // grpStream
            // 
            this.grpStream.Controls.Add(this.lblClientCount);
            this.grpStream.Controls.Add(this.lblClientsCaption);
            this.grpStream.Controls.Add(this.lblStatus);
            this.grpStream.Controls.Add(this.lblStatusCaption);
            this.grpStream.Controls.Add(this.btnOpenBrowser);
            this.grpStream.Controls.Add(this.btnCopyUrl);
            this.grpStream.Controls.Add(this.txtStreamUrl);
            this.grpStream.Controls.Add(this.lblStreamUrlCaption);
            this.grpStream.Location = new System.Drawing.Point(12, 395);
            this.grpStream.Name = "grpStream";
            this.grpStream.Size = new System.Drawing.Size(440, 115);
            this.grpStream.TabIndex = 4;
            this.grpStream.TabStop = false;
            this.grpStream.Text = "Stream Information";
            // 
            // lblStreamUrlCaption
            // 
            this.lblStreamUrlCaption.AutoSize = true;
            this.lblStreamUrlCaption.Location = new System.Drawing.Point(15, 25);
            this.lblStreamUrlCaption.Name = "lblStreamUrlCaption";
            this.lblStreamUrlCaption.Size = new System.Drawing.Size(68, 13);
            this.lblStreamUrlCaption.TabIndex = 0;
            this.lblStreamUrlCaption.Text = "Stream URL:";
            // 
            // txtStreamUrl
            // 
            this.txtStreamUrl.Location = new System.Drawing.Point(90, 22);
            this.txtStreamUrl.Name = "txtStreamUrl";
            this.txtStreamUrl.ReadOnly = true;
            this.txtStreamUrl.Size = new System.Drawing.Size(245, 20);
            this.txtStreamUrl.TabIndex = 1;
            this.txtStreamUrl.Text = "http://127.0.0.1:9000";
            // 
            // btnCopyUrl
            // 
            this.btnCopyUrl.Location = new System.Drawing.Point(345, 20);
            this.btnCopyUrl.Name = "btnCopyUrl";
            this.btnCopyUrl.Size = new System.Drawing.Size(80, 25);
            this.btnCopyUrl.TabIndex = 2;
            this.btnCopyUrl.Text = "Copy URL";
            this.btnCopyUrl.UseVisualStyleBackColor = true;
            this.btnCopyUrl.Click += new System.EventHandler(this.btnCopyUrl_Click);
            // 
            // btnOpenBrowser
            // 
            this.btnOpenBrowser.Location = new System.Drawing.Point(90, 50);
            this.btnOpenBrowser.Name = "btnOpenBrowser";
            this.btnOpenBrowser.Size = new System.Drawing.Size(130, 25);
            this.btnOpenBrowser.TabIndex = 3;
            this.btnOpenBrowser.Text = "Open in Browser";
            this.btnOpenBrowser.UseVisualStyleBackColor = true;
            this.btnOpenBrowser.Click += new System.EventHandler(this.btnOpenBrowser_Click);
            // 
            // lblStatusCaption
            // 
            this.lblStatusCaption.AutoSize = true;
            this.lblStatusCaption.Location = new System.Drawing.Point(15, 57);
            this.lblStatusCaption.Name = "lblStatusCaption";
            this.lblStatusCaption.Size = new System.Drawing.Size(40, 13);
            this.lblStatusCaption.TabIndex = 4;
            this.lblStatusCaption.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblStatus.ForeColor = System.Drawing.Color.Gray;
            this.lblStatus.Location = new System.Drawing.Point(15, 75);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(54, 13);
            this.lblStatus.TabIndex = 5;
            this.lblStatus.Text = "Stopped";
            // 
            // lblClientsCaption
            // 
            this.lblClientsCaption.AutoSize = true;
            this.lblClientsCaption.Location = new System.Drawing.Point(250, 57);
            this.lblClientsCaption.Name = "lblClientsCaption";
            this.lblClientsCaption.Size = new System.Drawing.Size(103, 13);
            this.lblClientsCaption.TabIndex = 6;
            this.lblClientsCaption.Text = "Connected Clients:";
            // 
            // lblClientCount
            // 
            this.lblClientCount.AutoSize = true;
            this.lblClientCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold);
            this.lblClientCount.Location = new System.Drawing.Point(290, 73);
            this.lblClientCount.Name = "lblClientCount";
            this.lblClientCount.Size = new System.Drawing.Size(20, 24);
            this.lblClientCount.TabIndex = 7;
            this.lblClientCount.Text = "0";
            // 
            // grpInstructions
            // 
            this.grpInstructions.Controls.Add(this.txtInstructions);
            this.grpInstructions.Location = new System.Drawing.Point(12, 518);
            this.grpInstructions.Name = "grpInstructions";
            this.grpInstructions.Size = new System.Drawing.Size(440, 120);
            this.grpInstructions.TabIndex = 5;
            this.grpInstructions.TabStop = false;
            this.grpInstructions.Text = "How to Use";
            // 
            // txtInstructions
            // 
            this.txtInstructions.BackColor = System.Drawing.SystemColors.Control;
            this.txtInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInstructions.Location = new System.Drawing.Point(15, 20);
            this.txtInstructions.Multiline = true;
            this.txtInstructions.Name = "txtInstructions";
            this.txtInstructions.ReadOnly = true;
            this.txtInstructions.Size = new System.Drawing.Size(410, 92);
            this.txtInstructions.TabIndex = 0;
            this.txtInstructions.Text = "1. Select a source: Desktop, Monitor, Application Window, or Webcam.\r\n" +
                "2. Configure resolution, FPS, and device-specific options.\r\n" +
                "3. Choose an IP address and port for the MJPEG server.\r\n" +
                "4. Click \"Start Streaming\" and open the URL in any browser.\r\n" +
                "5. To embed: <img src=\"http://IP:PORT\" />\r\n" +
                "6. To stream a browser tab, select it from \"Application Window\".";
            // 
            // tmrClientCount
            // 
            this.tmrClientCount.Interval = 1000;
            this.tmrClientCount.Tick += new System.EventHandler(this.tmrClientCount_Tick);
            // 
            // ScreenForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 648);
            this.Controls.Add(this.grpInstructions);
            this.Controls.Add(this.grpStream);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.grpServer);
            this.Controls.Add(this.grpSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "ScreenForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tarsier ShareScreen - MJPEG Streaming Demo";
            this.Load += new System.EventHandler(this.ScreenForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScreenForm_FormClosing);
            this.grpSource.ResumeLayout(false);
            this.grpSource.PerformLayout();
            this.grpServer.ResumeLayout(false);
            this.grpServer.PerformLayout();
            this.grpStream.ResumeLayout(false);
            this.grpStream.PerformLayout();
            this.grpInstructions.ResumeLayout(false);
            this.grpInstructions.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.Label lblSourceType;
        private System.Windows.Forms.ComboBox cboSourceType;
        private System.Windows.Forms.Label lblResolution;
        private System.Windows.Forms.ComboBox cboResolution;
        private System.Windows.Forms.Label lblFps;
        private System.Windows.Forms.ComboBox cboFps;
        private System.Windows.Forms.Label lblMonitor;
        private System.Windows.Forms.ComboBox cboMonitor;
        private System.Windows.Forms.Button btnRefreshMonitors;
        private System.Windows.Forms.Label lblWindow;
        private System.Windows.Forms.ComboBox cboWindows;
        private System.Windows.Forms.Button btnRefreshWindows;
        private System.Windows.Forms.Label lblWebcam;
        private System.Windows.Forms.ComboBox cboWebcam;
        private System.Windows.Forms.Button btnRefreshWebcams;
        private System.Windows.Forms.CheckBox chkShowCursor;
        private System.Windows.Forms.GroupBox grpServer;
        private System.Windows.Forms.Label lblIpAddress;
        private System.Windows.Forms.ComboBox cboIpAddress;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.GroupBox grpStream;
        private System.Windows.Forms.Label lblStreamUrlCaption;
        private System.Windows.Forms.TextBox txtStreamUrl;
        private System.Windows.Forms.Button btnCopyUrl;
        private System.Windows.Forms.Button btnOpenBrowser;
        private System.Windows.Forms.Label lblStatusCaption;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblClientsCaption;
        private System.Windows.Forms.Label lblClientCount;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.GroupBox grpInstructions;
        private System.Windows.Forms.TextBox txtInstructions;
        private System.Windows.Forms.Timer tmrClientCount;
    }
}

