using System.Collections.Generic;
using System.Net.Sockets;

namespace Tarsier.ShareScreen.Core.Extensions
{
    /// <summary>
    /// Provides an enumerated connection in an enumerated form.
    /// </summary>
    internal static class SocketExtensions
    {
        /// <summary>
        /// Provides an enumerated connection in an enumerated form.
        /// </summary>
        /// <param name="server">Server socket.</param>
        /// <returns>An enumerated connection in an enumerated form.</returns>
        public static IEnumerable<Socket> GetIncomingConnections(this Socket server) {
            while (true) {
                yield return server.Accept();
            }
        }
    }
}
