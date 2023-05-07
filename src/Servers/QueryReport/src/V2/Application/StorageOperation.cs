using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Abstraction.Interface;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using System.Linq;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis;
using UniSpy.Server.Core.Database.DatabaseModel;
using System.Net;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Application
{
    public sealed class StorageOperation : IStorageOperation
    {
        private static QueryReport.V2.Aggregate.Redis.GameServer.RedisClient _gameServerRedisClient = new QueryReport.V2.Aggregate.Redis.GameServer.RedisClient();
        public static IStorageOperation Persistance = new StorageOperation();
        public static NatNegChannel NatNegChannel = new NatNegChannel();
        ///<summary>
        /// we do not run subscribe() in QR because QR only need to push.
        /// We run subscribe() in SB, because SB need to receive message.
        ///</summary>
        public static HeartbeatChannel HeartbeatChannel = new HeartbeatChannel();
        public List<GameServerInfo> GetServerInfos(uint instantKey) => _gameServerRedisClient.Context.Where(x => x.InstantKey == instantKey).ToList();

        public void RemoveGameServer(GameServerInfo info) => _gameServerRedisClient.DeleteKeyValue(info);
        public void UpdateGameServer(GameServerInfo info) => _ = _gameServerRedisClient.SetValueAsync(info);
        public List<GameServerInfo> GetGameServerInfos(string gameName)
        {
            return _gameServerRedisClient.Context.Where(x => x.GameName == gameName).ToList();
        }

        public Dictionary<string, List<Grouplist>> GetAllGroupList()
        {
            using (var db = new UniSpyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplists on g.Gameid equals gl.Gameid
                             select new Grouplist
                             {
                                 Game = g,
                                 Gameid = g.Gameid,
                                 Groupid = gl.Groupid,
                                 Roomname = gl.Roomname
                             };
                var result2 = from g in result
                              group g by g.Game.Gamename into dd
                              select new KeyValuePair<string, List<Grouplist>>(dd.Key, dd.ToList());

                var data = result2.ToDictionary(x => x.Key, x => x.Value);
                return data;
            }
        }
        public GameServerInfo GetGameServerInfo(IPEndPoint end)
        {
            return _gameServerRedisClient.Context.FirstOrDefault(x =>
                x.HostIPAddress == end.Address &
                x.QueryReportPort == (ushort)end.Port);
        }

        // publish client message here
        public void PublishClientMessage(ClientMessageRequest message)
        {
            NatNegChannel.PublishMessage(message);
        }
    }
}