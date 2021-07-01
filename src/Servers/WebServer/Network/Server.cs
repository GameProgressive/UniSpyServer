using System.Net;
using NetCoreServer;

namespace WebServer.Network
{
    internal class Server : HttpServer
    {
        public Server(IPAddress address, int port) : base(address, port)
        {
            
        }
    }
}