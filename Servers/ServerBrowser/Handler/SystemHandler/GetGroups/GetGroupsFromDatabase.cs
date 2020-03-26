using System;
using System.Collections.Generic;
using GameSpyLib.Database.DatabaseModel.MySql;
using ServerBrowser.Entity.Interface;
using System.Linq;

namespace ServerBrowser.Handler.SystemHandler.GetGroups
{
    public class GetGroupsFromDatabase : IGetGroupAble
    {
        public IEnumerable<Grouplist> GetAvailableGroup(string gameName)
        {
            using (var db = new retrospyContext())
            {
                return from g in db.Grouplist
                       join gn in db.Games on g.Gameid equals gn.Id
                       where gn.Gamename == gameName
                       select g;
            }
        }
    }
}
