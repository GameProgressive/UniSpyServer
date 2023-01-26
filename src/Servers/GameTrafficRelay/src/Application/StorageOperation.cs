using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.GameTrafficRelay.Entity;
using UniSpyServer.Servers.GameTrafficRelay.Interface;

namespace UniSpyServer.Servers.GameTrafficRelay.Application
{
    public sealed class StorageOperation : IStorageOperation
    {
        /// <summary>
        /// Game relay server information redis server.
        /// </summary>
        private static RedisClient _redisClient = new RedisClient();

        public static IStorageOperation Persistance = new StorageOperation();
        public List<RelayServerInfo> GetAvaliableRelayServers()
        {
            return _redisClient.Context.ToList();
        }
    }
}