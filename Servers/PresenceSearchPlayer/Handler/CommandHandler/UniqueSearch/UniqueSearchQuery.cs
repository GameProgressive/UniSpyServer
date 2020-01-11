using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.UniqueSearch
{
    public class UniqueSearchQuery
    {
        public static bool IsUniqueNickExist(string uniquenick, uint namespaceid)
        {
            var result = GPSPServer.DB.Query("SELECT uniquenick FROM namespace " +
                 "WHERE uniquenick=@P0 AND namespaceid=@P1",
                uniquenick, namespaceid)[0];

            if (result == null)
                return true;
            else
                return false;
        }
    }
}
