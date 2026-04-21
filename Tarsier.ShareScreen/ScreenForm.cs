using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using Tarsier.ShareScreen.Core.Enumerations;
using Tarsier.ShareScreen.Core.NativeAPI;
using Tarsier.ShareScreen.Core.Server;
using Tarsier.ShareScreen.Core.Snapshots.Webcam;

namespace Tarsier.ShareScreen
{
    public partial class ScreenForm : Form
    {
        private StreamingServer _streamingServer;
        private (string Name, string MonikerString)[] _webcamDevices;

        public ScreenForm() {
            InitializeComponent();
        }

        private void ScreenForm_Load(object sender, EventArgs e) {
            PopulateSourceTypes();
            PopulateResolutions();
            PopulateFpsOptions();
            PopulateIpAddresses();
            PopulateMonitors();
            RefreshWindowList();
            RefreshWebcamList();
            UpdateSourceUI();
            UpdateStreamUrl();
        }

        private void ScreenForm_FormClosing(object sender, FormClosingEventArgs e) {
            StopStreaming();
        }

        #region Populate Controls

        private void PopulateSourceTypes() {
            cboSourceType.Items.Clear();
            cboSourceType.Items.Add("Desktop Screen (Primary)");
            cboSourceType.Items.Add("Monitor");
            cboSourceType.Items.Add("Application Window");
            cboSourceType.Items.Add("Webcam");
            cboSourceType.SelectedIndex = 0;
        }

        private void PopulateResolutions() {
            cboResolution.Items.Clear();
            cboResolution.Items.Add("1080p (1920×1080)");
            cboResolution.Items.Add("720p (1080×720)");
            cboResolution.Items.Add("480p (854×480)");
            cboResolution.Items.Add("360p (480×360)");
            cboResolution.Items.Add("240p (352×240)");
            cboResolution.SelectedIndex = 0;
        }

        private void PopulateFpsOptions() {
            cboFps.Items.Clear();
            cboFps.Items.Add("120 FPS");
            cboFps.Items.Add("60 FPS");
            cboFps.Items.Add("30 FPS");
            cboFps.Items.Add("15 FPS");
            cboFps.SelectedIndex = 2; // Default to 30 FPS
        }

        private void PopulateIpAddresses() {
            cboIpAddress.Items.Clear();
            cboIpAddress.Items.Add("127.0.0.1 (Localhost)");
            cboIpAddress.Items.Add("0.0.0.0 (All Interfaces)");

            try {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (var ip in host.AddressList) {
                    if (ip.AddressFamily == AddressFamily.InterNetwork) {
                        cboIpAddress.Items.Add(ip.ToString());
                    }
                }
            } catch {
                // Fallback: just use localhost
            }

            cboIpAddress.SelectedIndex = 0;
        }

        private void PopulateMonitors() {
            cboMonitor.Items.Clear();
            var screens = Screen.AllScreens;
            for (int i = 0; i < screens.Length; i++) {
                var s = screens[i];
                string primary = s.Primary ? " (Primary)" : "";
                cboMonitor.Items.Add($"Monitor {i + 1}: {s.Bounds.Width}×{s.Bounds.Height}{primary}");
            }
            if (cboMonitor.Items.Count > 0) {
                cboMonitor.SelectedIndex = 0;
            }
        }

        private void RefreshWindowList() {
            cboWindows.Items.Clear();
            try {
                foreach (var appTitle in RunningApplications.GetListOfRunningApps()) {
                    if (!string.IsNullOrWhiteSpace(appTitle) && appTitle != Text) {
                        cboWindows.Items.Add(appTitle);
                    }
                }
            } catch {
                // Ignore errors enumerating windows
            }

            if (cboWindows.Items.Count > 0) {
                cboWindows.SelectedIndex = 0;
            }
        }

        private void RefreshWebcamList() {
            cboWebcam.Items.Clear();
            try {
                _webcamDevices = HeadlessWebcamCapture.GetAvailableDevices();
                foreach (var device in _webcamDevices) {
                    cboWebcam.Items.Add(device.Name);
                }
            } catch {
                _webcamDevices = new (string, string)[0];
            }

            if (cboWebcam.Items.Count > 0) {
                cboWebcam.SelectedIndex = 0;
            }
        }

        #endregion

        #region UI Update Helpers

        private void UpdateSourceUI() {
            int sourceIndex = cboSourceType.SelectedIndex;

            bool isDesktop = sourceIndex == 0;   // Desktop Screen (Primary)
            bool isMonitor = sourceIndex == 1;    // Monitor
            bool isWindow = sourceIndex == 2;     // Application Window
            bool isCamera = sourceIndex == 3;     // Webcam

            // Resolution applies to desktop and monitor
            cboResolution.Enabled = isDesktop || isMonitor;
            lblResolution.Enabled = isDesktop || isMonitor;

            // Monitor selection
            cboMonitor.Enabled = isMonitor;
            lblMonitor.Enabled = isMonitor;
            btnRefreshMonitors.Enabled = isMonitor;

            // Window selection
            cboWindows.Enabled = isWindow;
            lblWindow.Enabled = isWindow;
            btnRefreshWindows.Enabled = isWindow;

            // Webcam selection
            cboWebcam.Enabled = isCamera;
            lblWebcam.Enabled = isCamera;
            btnRefreshWebcams.Enabled = isCamera;

            // Show cursor option
            chkShowCursor.Enabled = isDesktop || isMonitor || isWindow;
        }

        private void UpdateStreamUrl() {
            string ip = GetSelectedIpAddress();
            string port = txtPort.Text.Trim();
            txtStreamUrl.Text = $"http://{ip}:{port}";
        }

        private string GetSelectedIpAddress() {
            if (cboIpAddress.SelectedItem == null) return "127.0.0.1";
            string selected = cboIpAddress.SelectedItem.ToString();

            if (selected.Contains("Localhost")) return "127.0.0.1";
            if (selected.Contains("All Interfaces")) {
                try {
                    var host = Dns.GetHostEntry(Dns.GetHostName());
                    var lanIp = host.AddressList.FirstOrDefault(a => a.AddressFamily == AddressFamily.InterNetwork);
                    if (lanIp != null) return lanIp.ToString();
                } catch { }
                return "127.0.0.1";
            }
            return selected;
        }

        private IPAddress GetBindIpAddress() {
            if (cboIpAddress.SelectedItem == null) return IPAddress.Loopback;
            string selected = cboIpAddress.SelectedItem.ToString();

            if (selected.Contains("Localhost")) return IPAddress.Loopback;
            if (selected.Contains("All Interfaces")) return IPAddress.Any;

            return IPAddress.TryParse(selected, out var ip) ? ip : IPAddress.Loopback;
        }

        private ScreenResolution GetSelectedResolution() {
            switch (cboResolution.SelectedIndex) {
                case 0: return ScreenResolution.OneThousandAndEightyP;
                case 1: return ScreenResolution.SevenHundredAndTwentyP;
                case 2: return ScreenResolution.FourHundredAndEightyP;
                case 3: return ScreenResolution.ThreeHundredAndSixtyP;
                case 4: return ScreenResolution.TwoHundredAndFortyP;
                default: return ScreenResolution.SevenHundredAndTwentyP;
            }
        }

        private Fps GetSelectedFps() {
            switch (cboFps.SelectedIndex) {
                case 0: return Fps.OneHundredAndTwenty;
                case 1: return Fps.Sixty;
                case 2: return Fps.Thirty;
                case 3: return Fps.Fifteen;
                default: return Fps.Thirty;
            }
        }

        private void SetStreamingState(bool isStreaming) {
            btnStart.Enabled = !isStreaming;
            btnStop.Enabled = isStreaming;

            grpSource.Enabled = !isStreaming;
            grpServer.Enabled = !isStreaming;

            if (isStreaming) {
                lblStatus.Text = "Streaming...";
                lblStatus.ForeColor = Color.Green;
                tmrClientCount.Start();
            } else {
                lblStatus.Text = "Stopped";
                lblStatus.ForeColor = Color.Gray;
                lblClientCount.Text = "0";
                tmrClientCount.Stop();
            }
        }

        #endregion

        #region Streaming Control

        private void StartStreaming() {
            try {
                int port;
                if (!int.TryParse(txtPort.Text.Trim(), out port) || port < 1 || port > 65535) {
                    MessageBox.Show("Please enter a valid port number (1-65535).", "Invalid Port",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Ensure any previous instance is cleared
                StreamingServer.ResetInstance();

                int sourceIndex = cboSourceType.SelectedIndex;
                Fps fps = GetSelectedFps();
                bool showCursor = chkShowCursor.Checked;

                switch (sourceIndex) {
                    case 0: // Desktop Screen (Primary)
                        ScreenResolution resolution = GetSelectedResolution();
                        _streamingServer = StreamingServer.GetInstance(resolution, fps, showCursor);
                        break;

                    case 1: // Monitor
                        if (cboMonitor.SelectedIndex < 0) {
                            MessageBox.Show("Please select a monitor.", "No Monitor Selected",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        int screenIndex = cboMonitor.SelectedIndex;
                        ScreenResolution monRes = GetSelectedResolution();
                        _streamingServer = StreamingServer.GetInstance(screenIndex, monRes, fps, showCursor);
                        break;

                    case 2: // Application Window
                        if (cboWindows.SelectedItem == null) {
                            MessageBox.Show("Please select an application window to stream.",
                                "No Window Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        string windowTitle = cboWindows.SelectedItem.ToString();
                        _streamingServer = StreamingServer.GetInstance(windowTitle, fps, showCursor);
                        break;

                    case 3: // Webcam
                        string monikerString = null;
                        if (_webcamDevices != null && cboWebcam.SelectedIndex >= 0
                            && cboWebcam.SelectedIndex < _webcamDevices.Length) {
                            monikerString = _webcamDevices[cboWebcam.SelectedIndex].MonikerString;
                        }
                        if (monikerString == null && (_webcamDevices == null || _webcamDevices.Length == 0)) {
                            MessageBox.Show("No webcam devices found.", "No Webcam",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        _streamingServer = StreamingServer.GetInstance(fps, monikerString);
                        break;

                    default:
                        return;
                }

                IPAddress bindAddress = GetBindIpAddress();
                _streamingServer.Start(bindAddress, port);
                SetStreamingState(true);
                UpdateStreamUrl();

            } catch (Exception ex) {
                MessageBox.Show($"Failed to start streaming:\n{ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                StopStreaming();
            }
        }

        private void StopStreaming() {
            try {
                if (_streamingServer != null) {
                    _streamingServer.Stop();
                    _streamingServer = null;
                }
                StreamingServer.ResetInstance();
            } catch {
                // Ensure UI resets even if stop fails
            }
            SetStreamingState(false);
        }

        #endregion

        #region Event Handlers

        private void cboSourceType_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateSourceUI();
            if (cboSourceType.SelectedIndex == 2) {
                RefreshWindowList();
            }
        }

        private void cboIpAddress_SelectedIndexChanged(object sender, EventArgs e) {
            UpdateStreamUrl();
        }

        private void txtPort_TextChanged(object sender, EventArgs e) {
            UpdateStreamUrl();
        }

        private void btnRefreshMonitors_Click(object sender, EventArgs e) {
            PopulateMonitors();
        }

        private void btnRefreshWindows_Click(object sender, EventArgs e) {
            RefreshWindowList();
        }

        private void btnRefreshWebcams_Click(object sender, EventArgs e) {
            RefreshWebcamList();
        }

        private void btnStart_Click(object sender, EventArgs e) {
            StartStreaming();
        }

        private void btnStop_Click(object sender, EventArgs e) {
            StopStreaming();
        }

        private void btnCopyUrl_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(txtStreamUrl.Text)) {
                Clipboard.SetText(txtStreamUrl.Text);
                MessageBox.Show("Stream URL copied to clipboard!", "Copied",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnOpenBrowser_Click(object sender, EventArgs e) {
            if (!string.IsNullOrEmpty(txtStreamUrl.Text)) {
                try {
                    Process.Start(new ProcessStartInfo {
                        FileName = txtStreamUrl.Text,
                        UseShellExecute = true
                    });
                } catch (Exception ex) {
                    MessageBox.Show($"Could not open browser:\n{ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void tmrClientCount_Tick(object sender, EventArgs e) {
            if (_streamingServer != null) {
                try {
                    lblClientCount.Text = _streamingServer.Clients.Count.ToString();
                } catch {
                    lblClientCount.Text = "0";
                }
            }
        }

        #endregion
    }
}
