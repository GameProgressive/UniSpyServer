using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Status
{
    public class StatusQuery
    {
        public static void UpdateStatus(Dictionary<string, string> recv, Guid guid)
        {
            var result = GPCMServer.DB.Query("SELECT profileid from namespace WHERE sessionkey = @P0 AND guid = @P1", recv["sesskey"], guid.ToString())[0];
            uint profileid = Convert.ToUInt32(result);
            GPCMServer.DB.Execute("UPDATE profiles SET status = @P0, statusstring=@P1, lastip=@P2 WHERE profileid=@P2 ", recv["status"], recv["statstring"], recv["locstring"], profileid);
        }
    }
}
