using System.Net;
using System.Linq;
using System.Net.NetworkInformation;

namespace UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay
{
    public sealed class NetworkUtils
    {
        /// <summary>
        /// Find 1 port is not using by other programs
        /// </summary>
        public static int GetAvailablePort(int startingPort)
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

            var port = Enumerable.Range(startingPort, ushort.MaxValue)
                .Where(i => !tcpConnectionPorts.Contains(i))
                .Where(i => !tcpListenerPorts.Contains(i))
                .Where(i => !udpListenerPorts.Contains(i))
                .First();

            return port;
        }
        public static IPEndPoint GetAvaliableLocalEndPoint(int startingPort = 1025)
        {
            var port = GetAvailablePort(startingPort);
            return new IPEndPoint(IPAddress.Any, port);
        }
    }
}