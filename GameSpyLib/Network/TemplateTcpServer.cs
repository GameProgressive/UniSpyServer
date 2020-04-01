using GameSpyLib.Common;
using NetCoreServer;
using System.Net;
using System.Net.Sockets;

namespace GameSpyLib.Network
{
    /// <summary>
    /// This is a template class that helps creating a TCP Server with logging functionality and ServerName, as required in the old network stack.
    /// </summary>
    public class TemplateTcpServer : TcpServer
    {
        /// <summary>
        /// The name of the server that is started, used primary in logging functions.
        /// </summary>
        public string ServerName { get; private set; }

        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TemplateTcpServer(string serverName, IPEndPoint endpoint) : base(endpoint)
        {
            ServerName = $"[{serverName}] ";
            Start();
        }

        /// <summary>
        /// Initialize TCP server with a given IP address and port number
        /// </summary>
        /// <param name="serverName">The name of the server that will be started</param>
        /// <param name="address">IP address</param>
        /// <param name="port">Port number</param>
        public TemplateTcpServer(string serverName, IPAddress address, int port) : base(address, port)
        {
            ServerName = $"[{serverName}] ";
            Start();
        }

        /// <summary>
        /// Handle error notification
        /// </summary>
        /// <param name="error">Socket error code</param>
        protected override void OnError(SocketError error)
        {
            ServerManagerBase.LogWriter.Log.Error(error.ToString());
        }

    }
}
