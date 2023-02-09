using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.V2.Abstraction.Interface;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.GameServer;
using System.Linq;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis;

namespace UniSpyServer.Servers.QueryReport.V2.Application
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