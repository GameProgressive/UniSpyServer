using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass.Redis;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Extensions;

namespace QueryReport.Entity.Structure.Redis
{
    public sealed class PeerGroupInfoRedisOperator :
        UniSpyRedisOperatorBase<PeerGroupInfoRedisKey, PeerGroupInfo>
    {
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
                    var searchKey = new PeerGroupInfoRedisKey()
                    {
                        GameName = gameName
                    };

                    var matchedKeys = GetMatchedKeys(searchKey);
                    if (matchedKeys.Count() != 0)
                    {
                        continue;
                    }
                    var gameRoom = LoadGameRooms(gameName);
                    var fullKey = new PeerGroupInfoRedisKey() { GameName = gameName };
                    SetKeyValue(fullKey, gameRoom);
                }
            }
        }
    }
}