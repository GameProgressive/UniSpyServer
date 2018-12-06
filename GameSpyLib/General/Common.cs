using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameSpyLib
{
    public enum GSIACResult
    {
        GSIACWaiting,                 // still waiting for a response from the backend
        GSIACAvailable,               // the game's backend services are available
        GSIACUnavailable,             // the game's backend services are unavailable
        GSIACTemporarilyUnavailable   // the game's backend services are temporarily unavailable
    }
    public abstract class Common
    {       
        public abstract void GSIStartAvailableCheck(string gamename);
        public abstract GSIACResult GSIAvailableCheckThink();
        public abstract void GSICancelAvailableCheck();
        GSIACResult _GSIACRsult;
        string __GSIACGamename;
    }
}
