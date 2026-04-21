using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Tarsier.ShareScreen.Core.Constants;
using Tarsier.ShareScreen.Core.Enumerations;
using Tarsier.ShareScreen.Core.Extensions;
using Tarsier.ShareScreen.Core.NativeAPI;
using Tarsier.ShareScreen.Core.Snapshots.Webcam;

namespace Tarsier.ShareScreen.Core.Snapshots
{
    /// <summary>
    /// Provides methods for creating screen snapshots.
    /// </summary>
    public static class Screenshot
    {
        /// <summary>
        /// Provides enumeration of screenshots from the primary screen.
        /// </summary>
        public static IEnumerable<Image> DesktopScreen(ScreenResolution requiredResolution, bool showCursor) {
            var screenSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            var requiredSize = requiredResolution.GetResolutionSize();

            var rawImage = new Bitmap(screenSize.Width, screenSize.Height);
            var rawGraphics = Graphics.FromImage(rawImage);

            bool isNeedToScale = screenSize != requiredSize;

            var image = rawImage;
            var graphics = rawGraphics;

            if (isNeedToScale) {
                image = new Bitmap(requiredSize.Width, requiredSize.Height);
                graphics = Graphics.FromImage(image);
            }

            var source = new Rectangle(0, 0, screenSize.Width, screenSize.Height);
            var destination = new Rectangle(0, 0, requiredSize.Width, requiredSize.Height);

            while (true) {
                rawGraphics.CopyFromScreen(0, 0, 0, 0, screenSize);

                if (showCursor) {
                    AddCursorToScreenshot(rawGraphics, source);
                }

                if (isNeedToScale) {
                    graphics.DrawImage(rawImage, destination, source, GraphicsUnit.Pixel);
                }

                yield return image;
            }
        }

        /// <summary>
        /// Provides enumeration of screenshots from a specific monitor by index.
        /// </summary>
        /// <param name="screenIndex">Zero-based index into Screen.AllScreens.</param>
        /// <param name="requiredResolution">Required screenshot resolution.</param>
        /// <param name="showCursor">Whether to display the cursor in screenshots.</param>
        public static IEnumerable<Image> MonitorScreen(int screenIndex, ScreenResolution requiredResolution, bool showCursor) {
            var screens = Screen.AllScreens;
            if (screenIndex < 0 || screenIndex >= screens.Length) {
                screenIndex = 0;
            }

            var screen = screens[screenIndex];
            var bounds = screen.Bounds;
            var screenSize = new Size(bounds.Width, bounds.Height);
            var requiredSize = requiredResolution.GetResolutionSize();

            var rawImage = new Bitmap(screenSize.Width, screenSize.Height);
            var rawGraphics = Graphics.FromImage(rawImage);

            bool isNeedToScale = screenSize != requiredSize;

            var image = rawImage;
            var graphics = rawGraphics;

            if (isNeedToScale) {
                image = new Bitmap(requiredSize.Width, requiredSize.Height);
                graphics = Graphics.FromImage(image);
            }

            var source = new Rectangle(0, 0, screenSize.Width, screenSize.Height);
            var destination = new Rectangle(0, 0, requiredSize.Width, requiredSize.Height);

            while (true) {
                rawGraphics.CopyFromScreen(bounds.X, bounds.Y, 0, 0, screenSize);

                if (showCursor) {
                    AddCursorToScreenshot(rawGraphics, new Rectangle(bounds.X, bounds.Y, screenSize.Width, screenSize.Height));
                }

                if (isNeedToScale) {
                    graphics.DrawImage(rawImage, destination, source, GraphicsUnit.Pixel);
                }

                yield return image;
            }
        }

        /// <summary>
        /// Provides enumeration of captured images from a webcam using headless DirectShow capture.
        /// </summary>
        /// <param name="deviceMonikerString">The moniker string of the webcam device, or null for the first available device.</param>
        public static IEnumerable<Image> WebCamera(string deviceMonikerString = null) {
            HeadlessWebcamCapture capture = null;
            try {
                // Find a device if none specified
                if (string.IsNullOrEmpty(deviceMonikerString)) {
                    var devices = HeadlessWebcamCapture.GetAvailableDevices();
                    if (devices.Length == 0) {
                        yield return PredefinedImage.Default();
                        yield break;
                    }
                    deviceMonikerString = devices[0].MonikerString;
                }

                capture = new HeadlessWebcamCapture(deviceMonikerString);
                capture.Start();

                // Give the camera a moment to initialize
                Thread.Sleep(500);

                while (true) {
                    var frame = capture.GetFrame();
                    if (frame != null) {
                        yield return frame;
                        frame.Dispose();
                    } else {
                        yield return PredefinedImage.Default();
                    }
                }
            } finally {
                capture?.Dispose();
            }
        }

        /// <summary>
        /// Provides enumeration of screenshots of a specific application window.
        /// </summary>
        public static IEnumerable<Image> AppWindow(string applicationName, bool showCursor) {
            var windowHandle = ApplicationWindow.FindWindow(null, applicationName);

            var screeRectangle = new Rectangle();

            while (true) {
                ApplicationWindow.GetWindowRect(windowHandle, ref screeRectangle);

                screeRectangle.Width -= screeRectangle.X;
                screeRectangle.Height -= screeRectangle.Y;

                var image = new Bitmap(screeRectangle.Width, screeRectangle.Height);

                var graphics = Graphics.FromImage(image);

                var hdc = graphics.GetHdc();

                if (!ApplicationWindow.PrintWindow(windowHandle, hdc,
                    ApplicationWindow.DrawAllWindow)) {
                    int error = Marshal.GetLastWin32Error();
                    throw new System.ComponentModel.Win32Exception($"An error occurred while creating a screenshot"
                        + $" of the application window. Error Number: {error}.");
                }

                graphics.ReleaseHdc(hdc);

                if (showCursor) {
                    AddCursorToScreenshot(graphics, screeRectangle);
                }

                yield return image;

                image.Dispose();
                graphics.Dispose();
            }
        }

        /// <summary>
        /// Adds a cursor to a screenshot.
        /// </summary>
        private static void AddCursorToScreenshot(Graphics graphics, Rectangle bounds) {
            if (graphics == null) {
                throw new ArgumentNullException(nameof(graphics));
            }

            MouseCursor.CursorInfo pci;
            pci.cbSize = Marshal.SizeOf(typeof(MouseCursor.CursorInfo));

            if (!MouseCursor.GetCursorInfo(out pci)) {
                return;
            }

            if (pci.flags != MouseCursor.CursorShowing) {
                return;
            }

            const int logicalWidth = 0;
            const int logicalHeight = 0;
            const int indexOfFrame = 0;

            MouseCursor.DrawIconEx(graphics.GetHdc(), pci.ptScreenPos.x - bounds.X,
                pci.ptScreenPos.y - bounds.Y, pci.hCursor, logicalWidth,
                logicalHeight, indexOfFrame, IntPtr.Zero, MouseCursor.DiNormal);

            graphics.ReleaseHdc();
        }
    }
}