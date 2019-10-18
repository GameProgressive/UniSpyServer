using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.UpdatePro.Query
{
    public class UpdateProQuery
    {
        public static void UpdateProfile(string sql)
        {
            GPCMServer.DB.Query(sql);
        }
    }
}
