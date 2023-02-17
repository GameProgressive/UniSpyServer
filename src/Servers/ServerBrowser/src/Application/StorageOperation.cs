
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Redis.GameServer;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Redis.PeerGroup;
using UniSpy.Server.QueryReport.V2.Entity.Structure.Request;
using UniSpy.Server.Core.Database.DatabaseModel;

namespace UniSpy.Server.ServerBrowser.Application
{
    internal sealed class StorageOperation : ServerBrowser.Abstraction.Interface.IStorageOperation
    {
        public static ServerBrowser.Abstraction.Interface.IStorageOperation Persistance = new StorageOperation();
        private static QueryReport.V2.Entity.Structure.Redis.GameServer.RedisClient _gameServerRedisClient = new QueryReport.V2.Entity.Structure.Redis.GameServer.RedisClient();
        private static QueryReport.V2.Entity.Structure.Redis.PeerGroup.RedisClient _peerGroupRedisClient = new QueryReport.V2.Entity.Structure.Redis.PeerGroup.RedisClient();
        /// <summary>
        /// The redis channel use to transfer client message
        /// </summary>
        /// <returns></returns>
        public static QueryReport.V2.Entity.Structure.Redis.RedisChannel Channel = new QueryReport.V2.Entity.Structure.Redis.RedisChannel();

        public List<GameServerInfo> GetGameServerInfos(string gameName)
        {
            return _gameServerRedisClient.Context.Where(x => x.GameName == gameName).ToList();
        }

        public PeerGroupInfo GetPeerGroupInfo(string gameName)
        {
            return _peerGroupRedisClient.Context.FirstOrDefault(x => x.GameName == gameName);
        }

        public List<Grouplist> GetGroupList(string gameName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from g in db.Games
                             join gl in db.Grouplists on g.Gameid equals gl.Gameid
                             where g.Gamename == gameName
                             select gl;
                return result.ToList();
            }

        }
        public void PublishClientMessage(ClientMessageRequest message)
        {
            Channel.PublishMessage(message);
        }

        public GameServerInfo GetGameServerInfo(IPEndPoint end)
        {
            return _gameServerRedisClient.Context.FirstOrDefault(x =>
                x.HostIPAddress == end.Address &
                x.QueryReportPort == (ushort)end.Port);
        }
    }
}