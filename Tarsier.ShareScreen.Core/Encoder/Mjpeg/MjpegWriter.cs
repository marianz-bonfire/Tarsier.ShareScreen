using System;
using System.IO;
using System.Text;

namespace Tarsier.ShareScreen.Core.Encoder.Mjpeg
{
    /// <summary>
    /// Provides a stream writer that can be used to write images as MJPEG 
    /// to any stream.
    /// </summary>
    internal class MjpegWriter : IDisposable
    {
        private Stream _stream;

        /// <summary>
        /// The constructor of the class that initializes the fields of the class.
        /// </summary>
        public MjpegWriter(Stream stream) {
            _stream = stream;
        }

        /// <summary>
        /// Writes response headers to a stream.
        /// </summary>
        public void WriteHeaders() {
            var headers = Encoding.ASCII.GetBytes(MjpegConstants.ResponseHeaders);

            const int offset = 0;
            _stream.Write(headers, offset, headers.Length);

            _stream.Flush();
        }

        /// <summary>
        /// Writes an image to a stream.
        /// </summary>
        /// <param name="imageStream">Memory stream of an image.</param>
        public void WriteImage(MemoryStream imageStream) {
            var headers = Encoding.ASCII.GetBytes(MjpegConstants.GetImageInfoHeaders(imageStream.Length));

            const int offset = 0;
            _stream.Write(headers, offset, headers.Length);

            imageStream.WriteTo(_stream);

            var endOfResponse = Encoding.ASCII.GetBytes(MjpegConstants.NewLine);

            _stream.Write(endOfResponse, offset, endOfResponse.Length);

            _stream.Flush();
        }

        public void Dispose() {
            try {
                _stream?.Dispose();
            } finally {
                _stream = null;
            }
        }
    }
}