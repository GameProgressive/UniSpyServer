using System.Collections.Generic;
using GameSpyLib.Database.DatabaseModel.MySql;
using ServerBrowser.Entity.Interface;
using ServerBrowser.Handler.SystemHandler.Filter;

namespace ServerBrowser.Handler.SystemHandler.GetGroups
{
    public class GetGroupsFromQR
    {

        public static IEnumerable<Grouplist> GetFilteredGroups(IGetGroupAble iGroup, string gameName, string filter)
        {
            //TODO
            //We filter group for next step
            return GroupFilter.FilterGroups(iGroup.GetAvailableGroup(gameName), filter);
        }

    }
}
