using System;
using System.Collections.Generic;
using GameSpyLib.Database.DatabaseModel.MySql;

namespace ServerBrowser.Handler.SystemHandler.Filter
{
    public class GroupFilter
    {
        public static IEnumerable<Grouplist> FilterGroups(IEnumerable<Grouplist> rawGroups, string filter)
        {
            return rawGroups;
        }
    }
}
