using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.CommandHandler.OthersList
{
    public class OthersListQuery
    {
        public static List<Dictionary<string, object>> GetOtherBuddyList(uint[] profileids, uint namespaceid)
        {
            var result = new List<Dictionary<string, object>>(profileids.Length);
            foreach (uint pid in profileids)
            {
                result.Add(GPSPServer.DB.Query(
                    @"SELECT profileid,uniquenick 
                    FROM namespace 
                    WHERE profileid = @P0 AND namespaceid =@P1",
                    pid, namespaceid)[0]);
            }
            return (result.Count == 0) ? null : result;
        }

    }
}
