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
        private static GameServerCache.RedisClient _gameServerRedisClient = new();
        public static IStorageOperation Persistance = new StorageOperation();
        public static NatNegChannel NatNegChannel = new NatNegChannel();
        ///<summary>
        /// we do not run subscribe() in QR because QR only need to push.
        /// We run subscribe() in SB, because SB need to receive message.
        ///</summary>
        public static HeartbeatChannel HeartbeatChannel = new HeartbeatChannel();
        public List<GameServerCache> GetServerInfos(uint instantKey) => _gameServerRedisClient.Context.Where(x => x.InstantKey == instantKey).ToList();

        public void RemoveGameServer(GameServerCache info) => _gameServerRedisClient.DeleteKeyValue(info);
        public void UpdateGameServer(GameServerCache info) => _ = _gameServerRedisClient.SetValueAsync(info);
        public List<GameServerCache> GetGameServerInfos(string gameName)
        {
            return _gameServerRedisClient.Context.Where(x => x.GameName == gameName).ToList();
        }

        public GameServerCache GetGameServerInfo(IPEndPoint end)
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