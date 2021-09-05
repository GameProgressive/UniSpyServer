using System.Collections.Generic;
using UniSpyLib.Database.DatabaseModel.MySql;

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
