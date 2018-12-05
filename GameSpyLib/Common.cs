using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSpyLib
{
    public static class Common
    {
        public enum GSIACResult
        {
            GSIACWaiting,                 // still waiting for a response from the backend
            GSIACAvailable,               // the game's backend services are available
            GSIACUnavailable,             // the game's backend services are unavailable
            GSIACTemporarilyUnavailable   // the game's backend services are temporarily unavailable
        }
        public const int PACKET_TYPE =  0x09;
        public const int MASTER_PORT = 27900;
        public const int MAX_RETRIES = 1;
        public const int TIMEOUT_TIME = 2000;
    }
}
