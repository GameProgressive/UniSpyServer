using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Structure;
using System;
using System.Collections.Generic;
using System.Net;

namespace PresenceConnectionManager.Handler.Status
{
    public class StatusQuery
    {
        /// <summary>
        /// update profile status
        /// </summary>
        /// <param name="profileid"></param>
        /// <param name="status"></param>
        /// <param name="statstring"></param>
        /// <param name="location"></param>
        /// <param name="lastIP"></param>
        public static void UpdateStatus(GPCMPlayerInfo playerInfo, uint status, string statstring, string location)
        {
            GPCMServer.DB.Execute("UPDATE profiles SET status = @P0, statstring=@P1, location=@P2 WHERE profileid=@P3", status, statstring, location, playerInfo.Profileid);
        }
       
    }
}
