using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.DatabaseQuery
{
    public class UpdateProQuery
    {
        public static void UpdateUserInfo(string query, object[] passData)
        {
           GPCMServer.DB.Query(query, passData);
        }
    }
}
