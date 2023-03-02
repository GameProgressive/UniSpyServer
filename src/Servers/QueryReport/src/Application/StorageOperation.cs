using System.Collections.Generic;
using UniSpy.Server.QueryReport.Abstraction.Interface;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;
using System.Linq;
using UniSpy.Server.QueryReport.Aggregate.Redis;

namespace UniSpy.Server.QueryReport.Application
{
    internal sealed class StorageOperation : IStorageOperation
    {
        private static RedisClient _redisClient = new RedisClient();
        public static IStorageOperation Persistance = new StorageOperation();
        // the launch of this channel is in UdpServer
        public static RedisChannel Channel = new RedisChannel();
        public List<GameServerInfo> GetServerInfos(uint instantKey)
        {
            return _redisClient.Context.Where(x => x.InstantKey == instantKey).ToList();
        }

        public void RemoveGameServer(GameServerInfo info)
        {
            _redisClient.DeleteKeyValue(info);
        }

        public void UpdateGameServer(GameServerInfo info)
        {
            _redisClient.SetValue(info);
        }
    }
}