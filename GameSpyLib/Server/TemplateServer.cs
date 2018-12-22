using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroSpyServer
{
    public abstract class TemplateServer
    {
        public  TemplateServer()
        {

        }
        /// <summary>
        /// start point of a server
        /// </summary>
        /// <param name="listeningIP">which ip should be listened</param>
        /// <param name="listeningPort">which port should be listened</param>
        public abstract void ServerInitial(string listeningIP, int listeningPort);
        /// <summary>
        /// connection to over server, exchange data.
        /// </summary>
        /// <param name="ServerName">the other server that data should be exchanged</param>
        public abstract void ConnectionBridge(string ServerName);
        

    }
}
