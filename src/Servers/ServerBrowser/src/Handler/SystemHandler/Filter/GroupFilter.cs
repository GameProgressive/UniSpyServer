using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.ServerBrowser.Handler.SystemHandler.Filter
{
    public class GroupFilter
    {
        public static IEnumerable<Grouplist> FilterGroups(IEnumerable<Grouplist> rawGroups, string filter)
        {
            return rawGroups;
        }
    }
}
