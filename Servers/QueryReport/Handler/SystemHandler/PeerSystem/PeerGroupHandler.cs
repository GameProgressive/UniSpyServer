using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Extensions;
using GameSpyLib.MiscMethod;
using QueryReport.Entity.Structure.Group;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QueryReport.Handler.SystemHandler.PeerSystem
{
    public class PeerGroupHandler
    {
        public List<PeerGroup> PeerGroup { get; protected set; }

        public PeerGroupHandler()
        {
            PeerGroup = new List<PeerGroup>();
        }

        /// <summary>
        /// Select specific game room
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public PeerGroup LoadGameRooms(string gameName)
        {
            using (var db = new retrospyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplist on g.Gameid equals gl.Gameid
                             where g.Gamename == gameName
                             select gl;
                PeerGroup group = new PeerGroup(result.First(), gameName);

                foreach (var r in result)
                {
                    PeerRoom room = new PeerRoom(r);
                    group.PeerRooms.Add(room);
                }
                return group;
            }
        }

        public void LoadAllGameGroupsToRedis()
        {
            using (var db = new retrospyContext())
            {
                var names = from gl in db.Grouplist
                            join g in db.Games on gl.Gameid equals g.Gameid
                            select g.Gamename;
                names = names.Distinct();

                foreach (var gameName in names)
                {
                    var result = RedisExtensions.SearchPeerGroupKeys(gameName);
                 
                    if(result.Count()!=0)
                    {
                        continue;
                    }
                    PeerGroup.Add(LoadGameRooms(gameName));
                    RedisExtensions.SetGroupList(gameName, LoadGameRooms(gameName));
                }
            }
        }
    }
}
