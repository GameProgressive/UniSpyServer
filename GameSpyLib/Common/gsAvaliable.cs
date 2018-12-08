using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace GameSpyLib.Common
{
    public class gsAvaliable
    {
        readonly int PACKET_TYPE = 0x09;
        readonly int MASTER_PORT = 27900;
        readonly int MAX_RETREIS = 1;
        readonly int TIMEOUT_TIME = 2000;
        /// <summary>
        /// these are possible return types for GSIAvailableCheckThink
        /// </summary>
        public  enum GSIACResult
        {
            /// <summary>
            /// still waiting for a response from the backend
            /// </summary>
            GSIACWaiting,
            /// <summary>
            ///  the game's backend services are available
            /// </summary>
            GSIACAvailable,
            /// <summary>
            ///  the game's backend services are unavailable
            /// </summary>
            GSIACUnavailable,
            /// <summary>
            /// the game's backend services are temporarily unavailable
            /// </summary>
            GSIACTemporarilyUnavailable
        };
        /// <summary>
        /// to see if they should communicate with the backend
        /// </summary>
        GSIACResult __GSIACResult = GSIACResult.GSIACWaiting;
        public int get_sockaddrin(string hostname, int port, IPEndPoint saddr)
        {
            return 1;
        }
    }
    class AC
    {
        Socket sock;
        IPEndPoint address;
        char[] packet=new char[64];
        int packetLen;
        ushort  sendTime;
        int retryCount;
    }
    
}
