using GameSpyLib.Database.DatabaseModel.MySql;
using System.Collections.Generic;

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
