using QueryReport.Entity.Structure.Group;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace QueryReport.Handler.SystemHandler.Redis
{
    public sealed class PeerGroupInfoRedisOperator : UniSpyRedisOperatorBase<PeerGroupInfo>
    {
        static PeerGroupInfoRedisOperator()
        {
            _dbNumber = RedisDBNumber.PeerGroup;
        }

        public static string BuildSearchKey(string gameName)
        {
            return UniSpyRedisOperatorBase<PeerGroupInfo>.BuildSearchKey(gameName);
        }

        public static string BuildFullKey(string gameName)
        {
            return UniSpyRedisOperatorBase<PeerGroupInfo>.BuildFullKey(gameName);
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
                    var searchKey = BuildSearchKey(gameName);
                    var matchedKeys = GetMatchedKeys(searchKey);
                    if (matchedKeys.Count() != 0)
                    {
                        continue;
                    }
                    var gameRoom = LoadGameRooms(gameName);
                    SetKeyValue(gameName, gameRoom);
                }
            }
        }
    }
}