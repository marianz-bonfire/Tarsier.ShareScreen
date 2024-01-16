using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Tarsier.ShareScreen.Core.Constants;
using Tarsier.ShareScreen.Core.Enumerations;
using Tarsier.ShareScreen.Core.Extensions;
using Tarsier.ShareScreen.Core.NativeAPI;
using Tarsier.ShareScreen.Core.Webcam.DirectX;
using static Tarsier.ShareScreen.Core.Webcam.DirectShow.Uuid;

namespace Tarsier.ShareScreen.Core.Snapshots
{
    /// <summary>
    /// Provides methods for creating screen snapshots.
    /// </summary>
    public static class Screenshot
    {
        /// <summary>
        /// Provides enumeration of screenshots.
        /// </summary>
        /// <param name="requiredResolution">Required screenshot resolution.</param>
        /// <param name="showCursor">Whether to display the cursor in screenshots.</param>
        /// <returns>Enumeration of screenshots.</returns>
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

        private static Image CapturePreviewImage { get; set; }

        /// <summary>
        /// Provides enumeration of captured images from Webcamera.
        /// </summary>
        /// <param name="requiredResolution">Required screenshot resolution.</param>
        /// <returns>Enumeration of camera captured images.</returns>
        public static IEnumerable<Image> WebCamera() {
            FilterCollection videoDevices = new FilterCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0) {
                CaptureWebcam camera = new CaptureWebcam(new Filter(videoDevices[0].MonikerString));
                camera.CaptureFrameEvent += (bitmap) => {
                    CapturePreviewImage = bitmap;
                };
                camera.StartPreview();
                while (true) {
                    CapturePreviewImage = camera.GetFrame();
                    //camera.PrepareCapture();
                    if (CapturePreviewImage != null) {
                        yield return CapturePreviewImage;
                    } else {

                        yield return PredefinedImage.Default();
                    }
                }
            } else {
                yield return PredefinedImage.Default();
            }
        }

        /// <summary>
        /// Provides enumeration of screenshots of a specific application window.
        /// </summary>
        /// <param name="applicationName">The title of the main application window.</param>
        /// <param name="showCursor">Whether to display the cursor in screenshots.</param>
        /// <returns>Enumeration of screenshots of a specific application window.</returns>
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
        /// <param name="graphics">Drawing surface.</param>
        /// <param name="bounds">Screen bounds.</param>
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