using UniSpyLib.Network;
using NetCoreServer;
using System.Net;

namespace GameStatus
{
    public class GSServer : TCPServerBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="databaseDriver">
        /// A connection to a database
        /// If the databaseDriver is null, then the server will attempt to create it's own connection
        /// otherwise it will use the specified connection
        /// </param>
        public GSServer(IPAddress address, int port) : base(address, port)
        {
        }

        protected override TcpSession CreateSession() { return new GSSession(this); }

    }
}
