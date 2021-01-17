using QueryReport.Entity.Structure.Group;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace QueryReport.Handler.SystemHandler.PeerSystem
{
    public static class PeerGroupLoader
    {
        private static List<PeerGroupInfo> _peerGroup;
        static PeerGroupLoader()
        {
            _peerGroup = new List<PeerGroupInfo>();
            LoadAllGameGroupsToRedis();
        }
        /// <summary>
        /// Select specific game room
        /// </summary>
        /// <param name="gameName"></param>
        /// <returns></returns>
        public static PeerGroupInfo LoadGameRooms(string gameName)
        {
            using (var db = new retrospyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplist on g.Gameid equals gl.Gameid
                             where g.Gamename == gameName
                             select gl;
                PeerGroupInfo group = new PeerGroupInfo(result.First(), gameName);

                foreach (var r in result)
                {
                    PeerRoomInfo room = new PeerRoomInfo(r);
                    group.PeerRooms.Add(room);
                }
                return group;
            }
        }

        public static void LoadAllGameGroupsToRedis()
        {
            using (var db = new retrospyContext())
            {
                var gameNames = from gl in db.Grouplist
                                join g in db.Games on gl.Gameid equals g.Gameid
                                select g.Gamename;
                gameNames = gameNames.Distinct();

                foreach (var gameName in gameNames)
                {
                    var searchKey = PeerGroupInfo.RedisOperator.BuildSearchKey(gameName);
                    var matchedKeys = PeerGroupInfo.RedisOperator.GetMatchedKeys(searchKey);
                    if (matchedKeys.Count() != 0)
                    {
                        continue;
                    }
                    _peerGroup.Add(LoadGameRooms(gameName));
                    var gameRoom = LoadGameRooms(gameName);
                    PeerGroupInfo.RedisOperator.SetKeyValue(gameName, gameRoom);
                }
            }
        }
    }
}
