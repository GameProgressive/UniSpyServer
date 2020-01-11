using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.Buddy.SendBlockList
{
    public class SendBlockListQuery
    {

        public static Dictionary<string, object> SearchBlockList(uint profileid, uint namespaceid)
        {
            var result = GPCMServer.DB.Query("SELECT targetid FROM blocked WHERE profileid = @P0 AND namespaceid = @P1", profileid, namespaceid);
            return (result.Count == 0) ? null : result[0];
        }
    }
}
