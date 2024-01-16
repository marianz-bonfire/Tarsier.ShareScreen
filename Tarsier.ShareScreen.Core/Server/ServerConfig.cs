using System.Net;

namespace Tarsier.ShareScreen.Core.Server
{
    /// <summary>
    /// Required configuration of the server
    /// </summary>
    internal class ServerConfig
    {
        public IPAddress IpAddress { get; }

        public int Port { get; }

        public ServerConfig(IPAddress ipAddress, int port) {
            IpAddress = ipAddress;
            Port = port;
        }
    }
}
