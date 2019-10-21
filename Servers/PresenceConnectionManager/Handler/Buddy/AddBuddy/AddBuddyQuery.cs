using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.Buddy.AddBuddy
{
    public class AddBuddyQuery
    {
        public static void SaveAddBuddyRequest(uint profileid,uint targetid,uint namespaceid,string reason)
        {
            var id = GPCMServer.DB.Query("SELECT id FROM addrequests WHERE profileid=@P0 AND targetid=@P1 AND namespaceid=@P2", profileid, targetid, namespaceid)[0];
            if (id.Count !=0)
            {
                return;
            }
            else 
            {
                GPCMServer.DB.Execute("UPDATE addrequests SET profileid=@P0, targetid=@P1, namespaceid=@P2, reason = @P3",profileid,targetid,namespaceid,reason);
            }            
        }
    }
}
