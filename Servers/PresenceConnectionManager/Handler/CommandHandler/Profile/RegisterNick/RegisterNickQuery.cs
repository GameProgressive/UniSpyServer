using System.Collections.Generic;

namespace PresenceConnectionManager.Profile.RegisterNick
{
    public class RegisterNickQuery
    {
        public static void UpdateUniquenick(string uniquenick, uint sesskey, uint partnerid)
        {
            GPCMServer.DB.Execute("UPDATE  namespace SET uniquenick=@P0 " +
                "WHERE sesskey=@P1 AND partnerid=@P2",
                uniquenick, sesskey, partnerid);
        }
    }
}
