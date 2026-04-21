using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Tarsier.ShareScreen.Core.Webcam.DirectShow;
using Tarsier.ShareScreen.Core.Webcam.DirectX;

namespace Tarsier.ShareScreen.Core.Snapshots.Webcam
{
    /// <summary>
    /// Headless webcam capture using DirectShow. Builds a minimal filter graph
    /// (Video Capture Device → Sample Grabber → Null Renderer) to grab frames
    /// without requiring a preview window.
    /// </summary>
    public class HeadlessWebcamCapture : IDisposable
    {
        // CLSID_NullRenderer {C1F400A4-3F08-11D3-9F0B-006008039E37}
        private static readonly Guid CLSID_NullRenderer = new Guid(0xC1F400A4, 0x3F08, 0x11D3, 0x9F, 0x0B, 0x00, 0x60, 0x08, 0x03, 0x9E, 0x37);

        private ExtendStreaming.IGraphBuilder _graphBuilder;
        private ExtendStreaming.ICaptureGraphBuilder2 _captureGraphBuilder;
        private CoreStreaming.IBaseFilter _videoDeviceFilter;
        private CoreStreaming.IBaseFilter _sampleGrabberFilter;
        private CoreStreaming.IBaseFilter _nullRendererFilter;
        private EditStreaming.ISampleGrabber _sampleGrabber;
        private ControlStreaming.IMediaControl _mediaControl;
        private EditStreaming.VideoInfoHeader _videoInfoHeader;
        private byte[] _frameBuffer;
        private bool _disposed;
        private bool _running;

        public int Width => _videoInfoHeader?.BmiHeader.Width ?? 0;
        public int Height => _videoInfoHeader?.BmiHeader.Height ?? 0;
        public bool IsRunning => _running;

        /// <summary>
        /// Creates a headless webcam capture from a filter's moniker string.
        /// </summary>
        public HeadlessWebcamCapture(string monikerString) {
            BuildGraph(monikerString);
        }

        /// <summary>
        /// Creates a headless webcam capture from a Core Filter.
        /// </summary>
        public HeadlessWebcamCapture(Filter filter) : this(filter.MonikerString) { }

        private void BuildGraph(string monikerString) {
            // Create filter graph
            _graphBuilder = (ExtendStreaming.IGraphBuilder)Activator.CreateInstance(
                Type.GetTypeFromCLSID(Uuid.Clsid.FilterGraph, true));

            // Create capture graph builder
            _captureGraphBuilder = (ExtendStreaming.ICaptureGraphBuilder2)Activator.CreateInstance(
                Type.GetTypeFromCLSID(Uuid.Clsid.CaptureGraphBuilder2, true));

            int hr = _captureGraphBuilder.SetFiltergraph(_graphBuilder);
            if (hr < 0) Marshal.ThrowExceptionForHR(hr);

            // Create and add video device filter
            _videoDeviceFilter = (CoreStreaming.IBaseFilter)Marshal.BindToMoniker(monikerString);
            hr = _graphBuilder.AddFilter(_videoDeviceFilter, "Video Capture Device");
            if (hr < 0) Marshal.ThrowExceptionForHR(hr);

            // Create and configure sample grabber
            var grabberType = Type.GetTypeFromCLSID(Uuid.Clsid.SampleGrabber);
            if (grabberType == null)
                throw new InvalidOperationException("DirectShow SampleGrabber is not registered.");

            var grabberObj = Activator.CreateInstance(grabberType);
            _sampleGrabber = (EditStreaming.ISampleGrabber)grabberObj;
            _sampleGrabberFilter = (CoreStreaming.IBaseFilter)grabberObj;

            // Set media type to RGB24
            var mediaType = new CoreStreaming.AmMediaType {
                majorType = Uuid.MediaType.Video,
                subType = Uuid.MediaSubType.RGB24,
                formatType = Uuid.FormatType.VideoInfo
            };

            hr = _sampleGrabber.SetMediaType(mediaType);
            if (hr < 0) Marshal.ThrowExceptionForHR(hr);

            hr = _graphBuilder.AddFilter(_sampleGrabberFilter, "Sample Grabber");
            if (hr < 0) Marshal.ThrowExceptionForHR(hr);

            // Create and add null renderer
            var nullRendererType = Type.GetTypeFromCLSID(CLSID_NullRenderer);
            if (nullRendererType == null)
                throw new InvalidOperationException("DirectShow NullRenderer is not registered.");

            _nullRendererFilter = (CoreStreaming.IBaseFilter)Activator.CreateInstance(nullRendererType);
            hr = _graphBuilder.AddFilter(_nullRendererFilter, "Null Renderer");
            if (hr < 0) Marshal.ThrowExceptionForHR(hr);

            // Render capture stream: VideoDevice → SampleGrabber → NullRenderer
            var cat = Uuid.PinCategory.Capture;
            var med = Uuid.MediaType.Video;
            hr = _captureGraphBuilder.RenderStream(cat, med, _videoDeviceFilter, _sampleGrabberFilter, _nullRendererFilter);
            if (hr < 0) Marshal.ThrowExceptionForHR(hr);

            // Get connected media type to determine frame dimensions
            var connectedType = new CoreStreaming.AmMediaType();
            hr = _sampleGrabber.GetConnectedMediaType(connectedType);
            if (hr < 0) Marshal.ThrowExceptionForHR(hr);

            if (connectedType.formatType != Uuid.FormatType.VideoInfo || connectedType.formatPtr == IntPtr.Zero)
                throw new NotSupportedException("Unsupported webcam media format.");

            _videoInfoHeader = (EditStreaming.VideoInfoHeader)Marshal.PtrToStructure(
                connectedType.formatPtr, typeof(EditStreaming.VideoInfoHeader));

            Marshal.FreeCoTaskMem(connectedType.formatPtr);
            connectedType.formatPtr = IntPtr.Zero;

            // Enable buffer sampling (pull mode)
            _sampleGrabber.SetBufferSamples(true);
            _sampleGrabber.SetOneShot(false);

            // Allocate frame buffer
            int bufferSize = _videoInfoHeader.BmiHeader.ImageSize;
            if (bufferSize <= 0) {
                bufferSize = _videoInfoHeader.BmiHeader.Width * _videoInfoHeader.BmiHeader.Height * 3;
            }
            _frameBuffer = new byte[bufferSize + 64000];

            // Get media control
            _mediaControl = (ControlStreaming.IMediaControl)_graphBuilder;
        }

        /// <summary>
        /// Starts the capture graph.
        /// </summary>
        public void Start() {
            if (_running) return;
            _mediaControl.Run();
            _running = true;
        }

        /// <summary>
        /// Stops the capture graph.
        /// </summary>
        public void Stop() {
            if (!_running) return;
            try { _mediaControl?.Stop(); } catch { }
            _running = false;
        }

        /// <summary>
        /// Gets the current frame as a Bitmap. Returns null if no frame is available yet.
        /// </summary>
        public Bitmap GetFrame() {
            if (!_running || _sampleGrabber == null || _frameBuffer == null)
                return null;

            try {
                int bufferSize = 0;
                _sampleGrabber.GetCurrentBuffer(ref bufferSize, IntPtr.Zero);

                if (bufferSize <= 0)
                    return null;

                var handle = GCHandle.Alloc(_frameBuffer, GCHandleType.Pinned);
                try {
                    var address = handle.AddrOfPinnedObject();
                    _sampleGrabber.GetCurrentBuffer(ref bufferSize, address);

                    int width = _videoInfoHeader.BmiHeader.Width;
                    int height = _videoInfoHeader.BmiHeader.Height;
                    int stride = width * 3;

                    // DIB bitmaps are stored bottom-up; adjust scan pointer
                    var scan0 = address + (height - 1) * stride;
                    var bitmap = new Bitmap(width, height, -stride, PixelFormat.Format24bppRgb, scan0);
                    return bitmap;
                } finally {
                    handle.Free();
                }
            } catch {
                return null;
            }
        }

        public void Dispose() {
            if (_disposed) return;
            _disposed = true;

            Stop();

            try {
                if (_videoDeviceFilter != null) {
                    _graphBuilder?.RemoveFilter(_videoDeviceFilter);
                    Marshal.ReleaseComObject(_videoDeviceFilter);
                }
            } catch { }

            try {
                if (_sampleGrabberFilter != null) {
                    _graphBuilder?.RemoveFilter(_sampleGrabberFilter);
                    Marshal.ReleaseComObject(_sampleGrabberFilter);
                }
            } catch { }

            try {
                if (_nullRendererFilter != null) {
                    _graphBuilder?.RemoveFilter(_nullRendererFilter);
                    Marshal.ReleaseComObject(_nullRendererFilter);
                }
            } catch { }

            try {
                if (_captureGraphBuilder != null)
                    Marshal.ReleaseComObject(_captureGraphBuilder);
            } catch { }

            try {
                if (_graphBuilder != null)
                    Marshal.ReleaseComObject(_graphBuilder);
            } catch { }

            _videoDeviceFilter = null;
            _sampleGrabberFilter = null;
            _nullRendererFilter = null;
            _sampleGrabber = null;
            _captureGraphBuilder = null;
            _graphBuilder = null;
            _mediaControl = null;
            _frameBuffer = null;
        }

        /// <summary>
        /// Lists available webcam device names and their moniker strings.
        /// </summary>
        public static (string Name, string MonikerString)[] GetAvailableDevices() {
            try {
                var filters = new FilterCollection(Uuid.FilterCategory.VideoInputDevice);
                var result = new (string, string)[filters.Count];
                for (int i = 0; i < filters.Count; i++) {
                    result[i] = (filters[i].Name, filters[i].MonikerString);
                }
                return result;
            } catch {
                return Array.Empty<(string, string)>();
            }
        }
    }
}
