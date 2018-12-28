using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace RetroSpyServer
{
    /// <summary>
    /// a server factory that create the instance of  servers
    /// </summary>
    public class ServerFactory
    {
        private GPSPServer gpsp_server;

        public ServerFactory()
        {
            gpsp_server = new GPSPServer(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 29901), 100);
        }
    }
}
