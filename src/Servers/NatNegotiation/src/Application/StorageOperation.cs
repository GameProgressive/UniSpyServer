using System;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.NatNegotiation.Abstraction.Interface;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;

namespace UniSpyServer.Servers.NatNegotiation.Application
{
    internal sealed class StorageOperation : IStorageOperation
    {
        /// <summary>
        /// natneg init information redis server.
        /// </summary>
        private static RedisClient _redisClient = new RedisClient();
        /// <summary>
        /// Game relay server information redis server.
        /// </summary>
        private static GameTrafficRelay.Entity.Structure.Redis.RedisClient _relayRedisClient = new GameTrafficRelay.Entity.Structure.Redis.RedisClient();

        public static IStorageOperation Persistance = new StorageOperation();
        public int CountInitInfo(uint cookie, byte version)
        {
            return _redisClient.Context.Count(k =>
                         k.Cookie == cookie
                         && k.Version == version);
        }

        public List<NatInitInfo> GetInitInfos(Guid serverId, uint cookie)
        {
            return _redisClient.Context.Where(k =>
                 k.ServerID == serverId
                 && k.Cookie == cookie).ToList();
        }

        public void UpdateInitInfo(NatInitInfo info)
        {
            _redisClient.SetValue(info);
        }

        public void RemoveInitInfo(NatInitInfo info)
        {
            _redisClient.DeleteKeyValue(info);
        }

        public List<GameTrafficRelay.Entity.Structure.Redis.RelayServerInfo> GetAvaliableRelayServers()
        {
            return _relayRedisClient.Context.ToList();
        }
    }
}