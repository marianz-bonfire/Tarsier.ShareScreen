using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Tarsier.ShareScreen.Core.Encoder.Mjpeg;
using Tarsier.ShareScreen.Core.Enumerations;
using Tarsier.ShareScreen.Core.Extensions;
using Tarsier.ShareScreen.Core.Snapshots;

namespace Tarsier.ShareScreen.Core.Server
{
    public class StreamingServer
    {
        private static readonly object SyncRoot = new object();
        private static StreamingServer _serverInstance;

        private IEnumerable<Image> _images;
        private Socket _serverSocket;
        private Thread _thread;

        public int Delay { get; }

        public List<Socket> Clients { get; }

        public bool IsRunning => _thread != null && _thread.IsAlive;

        /// <summary>
        /// Initializes the fields and properties of the class for the screen stream.
        /// </summary>
        private StreamingServer(ScreenResolution imageResolution, Fps fps, bool showCursor)
            : this(Screenshot.DesktopScreen(imageResolution, showCursor), fps) {

        }

        /// <summary>
        /// Initializes the fields and properties of the class to stream a specific application window.
        /// </summary>
        private StreamingServer(string applicationName, Fps fps, bool showCursor)
            : this(Screenshot.AppWindow(applicationName, showCursor), fps) {
        }

        /// <summary>
        /// Initializes the fields and properties of the class to stream a specific application window.
        /// </summary>
        private StreamingServer(Fps fps)
            : this(Screenshot.WebCamera(), fps) {
        }

        /// <summary>
        /// The constructor of the class that initializes the fields of the class.
        /// </summary>
        private StreamingServer(IEnumerable<Image> images, Fps fps) {
            _thread = null;
            _images = images;

            Clients = new List<Socket>();
            Delay = (int)fps;
        }

        /// <summary>
        /// Provides a server object for a screen stream.
        /// </summary>
        /// <param name="resolutions">Stream Resolution.</param>
        /// <param name="fps">FPS.</param>
        /// <param name="showCursor">Whether to display the cursor in screenshots.</param>
        /// <returns>The object of the StreamingServer class.</returns>
        public static StreamingServer GetInstance(ScreenResolution resolutions, Fps fps, bool showCursor) {
            lock (SyncRoot) {
                if (_serverInstance == null) {
                    _serverInstance = new StreamingServer(resolutions, fps, showCursor);
                }
            }

            return _serverInstance;
        }

        /// <summary>
        /// Provides an object for a window stream of a specific application.
        /// </summary>
        /// <param name="applicationName">The title of the main application window.</param>
        /// <param name="fps">FPS.</param>
        /// <param name="showCursor">Whether to display the cursor in screenshots.</param>
        /// <returns>The object of the StreamingServer class.</returns>
        public static StreamingServer GetInstance(string applicationName, Fps fps, bool showCursor) {
            lock (SyncRoot) {
                if (_serverInstance == null) {
                    _serverInstance = new StreamingServer(applicationName, fps, showCursor);
                }
            }

            return _serverInstance;
        }

        /// <summary>
        /// Provides an object for a window stream of a specific application.
        /// </summary>
        /// <param name="fps">FPS.</param>
        /// <returns>The object of the StreamingServer class.</returns>
        public static StreamingServer GetInstance(Fps fps) {
            lock (SyncRoot) {
                if (_serverInstance == null) {
                    _serverInstance = new StreamingServer(fps);
                }
            }

            return _serverInstance;
        }

        /// <summary>
        /// Starts the server on the specified port.
        /// </summary>
        /// <param name="ipAddress">The IP address on which to start the server.</param>
        /// <param name="port">Server port.</param>
        public void Start(IPAddress ipAddress, int port) {
            var serverConfig = new ServerConfig(ipAddress, port);

            lock (this) {
                _thread = new Thread(StartServerThread) {
                    IsBackground = true
                };

                _thread.Start(serverConfig);
            }
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop() {
            if (!IsRunning) {
                return;
            }

            try {
                _serverSocket.Shutdown(SocketShutdown.Both);
            } catch {
                _serverSocket.Close();
            } finally {
                _thread = null;
                _images = null;
                _serverInstance = null;
            }
        }

        /// <summary>
        /// Starts the server in a separate thread.
        /// </summary>
        /// <param name="config">IP address and port on which you want to start the server.</param>
        private void StartServerThread(object config) {
            var serverConfig = (ServerConfig)config;

            try {
                _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                _serverSocket.Bind(new IPEndPoint(serverConfig.IpAddress, serverConfig.Port));
                _serverSocket.Listen(10);

                foreach (var client in _serverSocket.GetIncomingConnections()) {
                    ThreadPool.QueueUserWorkItem(StartClientThread, client);
                }
            } catch (SocketException) {
                foreach (var client in Clients.ToArray()) {
                    try {
                        client.Shutdown(SocketShutdown.Both);
                    } catch (ObjectDisposedException) {
                        client.Close();
                    }

                    Clients.Remove(client);
                }
            }
        }

        /// <summary>
        /// Starts a thread to handle clients.
        /// </summary>
        /// <param name="client">Client socket.</param>
        private void StartClientThread(object client) {
            var clientSocket = (Socket)client;
            clientSocket.SendTimeout = 10000;
            if (!Clients.Contains(clientSocket)) {
                Clients.Add(clientSocket);
            }


            try {
                using (var mjpegWriter = new MjpegWriter(new NetworkStream(clientSocket, true))) {
                    // Writes the response header to the client.
                    mjpegWriter.WriteHeaders();

                    // Streams the images from the source to the client.
                    foreach (var imgStream in _images.GetMjpegStream()) {
                        Thread.Sleep(Delay);

                        mjpegWriter.WriteImage(imgStream);
                    }
                }
            } catch (SocketException) {

            } catch (IOException) {

            } catch (Exception) {

            } finally {
                try {
                    clientSocket.Shutdown(SocketShutdown.Both);
                } catch (ObjectDisposedException) {
                    clientSocket.Close();
                }

                lock (Clients) {
                    Clients.Remove(clientSocket);
                }
            }
        }
    }
}