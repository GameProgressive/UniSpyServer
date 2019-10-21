using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.Buddy.AddBlock
{
    public class AddBlockQuery
    {
        public static void UpdateBlockListInDatabase(uint profileid,uint targetid, uint namespaceid)
        {
            var id = GPCMServer.DB.Query("SELECT id FROM blocked WHERE profileid=@P0 AND targetid=@P1 and namespaceid=@P2",profileid,targetid,namespaceid)[0];
            if (id.Count == 0)
            {
                GPCMServer.DB.Execute("UPDATE blocked SET profileid=@P0, targetid=@P1, namespaceid=@P2", profileid, targetid, namespaceid);
            }
            else
            {
                return;
            }
        }
    }
    
}
