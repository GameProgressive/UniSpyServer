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
        /// <param name="statuscode"></param>
        /// <param name="status"></param>
        /// <param name="location"></param>
        /// <param name="lastIP"></param>
        public static void UpdateStatus(GPCMPlayerInfo playerInfo, uint statuscode, string status, string location)
        {
            GPCMServer.DB.Execute("UPDATE profiles SET statuscode = @P0, status=@P1, location=@P2 WHERE profileid=@P3", statuscode, status, location, playerInfo.Profileid);
        }
       
    }
}
