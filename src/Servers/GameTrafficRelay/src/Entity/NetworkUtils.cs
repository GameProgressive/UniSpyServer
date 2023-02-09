using System.Linq;
using System.Net.NetworkInformation;

namespace UniSpyServer.Servers.GameTrafficRelay.Entity
{
    public sealed class NetworkUtils
    {
        /// <summary>
        /// Find 2 ports is not using by other programs
        /// </summary>
        /// <param name="startingPort"></param>
        /// <returns>ports array</returns>
        public static int[] GetAvailablePorts(int startingPort = 1025)
        {
            var properties = IPGlobalProperties.GetIPGlobalProperties();

            //getting active connections
            var tcpConnectionPorts = properties.GetActiveTcpConnections()
                                .Where(n => n.LocalEndPoint.Port >= startingPort)
                                .Select(n => n.LocalEndPoint.Port);

            //getting active tcp listners - WCF service listening in tcp
            var tcpListenerPorts = properties.GetActiveTcpListeners()
                                .Where(n => n.Port >= startingPort)
                                .Select(n => n.Port);

            //getting active udp listeners
            var udpListenerPorts = properties.GetActiveUdpListeners()
                                .Where(n => n.Port >= startingPort)
                                .Select(n => n.Port);

            var ports = Enumerable.Range(startingPort, ushort.MaxValue)
                .Where(i => !tcpConnectionPorts.Contains(i))
                .Where(i => !tcpListenerPorts.Contains(i))
                .Where(i => !udpListenerPorts.Contains(i))
                .Take(2).ToArray();

            return ports;
        }
    }
}