using System;
using System.Collections.Generic;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using RetroSpyServer.DBQueries;
using RetroSpyServer.Servers.GPCM.Enumerator;
using RetroSpyServer.Servers.GPCM.Structures;
using RetroSpyServer.Servers.GPSP.Enumerators;

namespace RetroSpyServer.Servers.GPCM
{
    /// <summary>
    /// This class contians gamespy GPCM functions  which help cdkeyserver to finish the GPCM functionality. 
    /// This class is used to simplify the functions in server class, separate the other utility function making  the main server logic clearer
    /// </summary>
    public class GPCMHandler
    {
        public static GPCMDBQuery DBQuery = null;

        public static void UpdateStatus(long timestamp, GPCMClient client)
        {
            DBQuery.UpdateStatus(timestamp, client.RemoteEndPoint.Address, client.PlayerInfo.PlayerId, (uint)client.PlayerInfo.PlayerStatus);
        }

        public static void ResetStatusAndSessionKey()
        {
            DBQuery.ResetStatusAndSessionKey();
        }




      

       
    }
}
