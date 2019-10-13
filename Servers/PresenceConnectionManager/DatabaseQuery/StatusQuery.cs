using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.DatabaseQuery
{
    public class StatusQuery
    {
        public static void UpdateStatus(Dictionary<string, string> recv , Guid guid)
        {
            var result = GPCMServer.DB.Query("SELECT profileid from namespace WHERE sessionkey = @P0 AND guid = @P1", recv["sesskey"],guid.ToString())[0];
            uint profileid = Convert.ToUInt32(result);
            GPCMServer.DB.Execute("UPDATE profiles SET statusstring=@P0, lastip=@P1 WHERE profileid=@P2 ", recv["statstring"], recv["locstring"], profileid);
        }
    }
}
