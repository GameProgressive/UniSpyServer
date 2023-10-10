using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.NatNegotiation.Abstraction.Interface;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;
using UniSpy.Server.NatNegotiation.Aggregate.Redis.Fail;

namespace UniSpy.Server.NatNegotiation.Application
{
    internal sealed class StorageOperation : IStorageOperation
    {
        /// <summary>
        /// natneg init information redis server.
        /// </summary>
        private InitPacketCache.RedisClient _redisClient = new();
        private RelayServerCache.RedisClient _relayRedisClient = new();
        private NatFailInfoCache.RedisClient _natFailRedisClient = new();
        public static IStorageOperation Persistance = new StorageOperation();
        public List<RelayServerCache> GetAvaliableRelayServers()
        {
            return _relayRedisClient.Context.ToList();
        }
        public int CountInitInfo(uint cookie, byte version)
        {
            return _redisClient.Context.Count(k =>
                         k.Cookie == cookie
                         && k.Version == version);
        }

        public List<Aggregate.Redis.InitPacketCache> GetInitInfos(Guid serverId, uint cookie)
        {
            return _redisClient.Context.Where(k =>
                 k.ServerID == serverId
                 && k.Cookie == cookie).ToList();
        }

        public void UpdateInitInfo(Aggregate.Redis.InitPacketCache info)
        {
            _ = _redisClient.SetValueAsync(info);
        }

        public void RemoveInitInfo(Aggregate.Redis.InitPacketCache info)
        {
            _redisClient.DeleteKeyValue(info);
        }

        public void UpdateNatFailInfo(Aggregate.Redis.Fail.NatFailInfoCache info)
        {
            _ = _natFailRedisClient.SetValueAsync(info);
        }

        public int GetNatFailInfo(Aggregate.Redis.Fail.NatFailInfoCache info)
        {
            return _natFailRedisClient.Context.Where(i => i.PublicIPAddress1.Equals(info.PublicIPAddress1) && i.PublicIPAddress2.Equals(info.PublicIPAddress2)).Count();
        }
    }
}