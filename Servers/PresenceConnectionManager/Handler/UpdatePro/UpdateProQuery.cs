using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.UpdatePro
{
    public class UpdateProQuery
    {
        public static void UpdateUserInfo(Dictionary<string,string> recv)
        {
           GPCMServer.DB.Query("UPDATE profiles SET firstname=@P0, lastname=@P1, icquin=@P2, homepage=@P3");
        }
    }
}
