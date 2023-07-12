using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.NatNegotiation.Abstraction.Interface;
using UniSpy.Server.NatNegotiation.Aggregate.GameTrafficRelay;

namespace UniSpy.Server.NatNegotiation.Application
{
    internal sealed class StorageOperation : IStorageOperation
    {
        /// <summary>
        /// natneg init information redis server.
        /// </summary>
        private static Aggregate.Redis.RedisClient _redisClient = new Aggregate.Redis.RedisClient();
        private static Aggregate.GameTrafficRelay.RedisClient _relayRedisClient = new Aggregate.GameTrafficRelay.RedisClient();
        private static NatNegotiation.Aggregate.Redis.Fail.RedisClient _natFailRedisClient = new NatNegotiation.Aggregate.Redis.Fail.RedisClient();
        public static IStorageOperation Persistance = new StorageOperation();
        public List<RelayServerInfo> GetAvaliableRelayServers()
        {
            return _relayRedisClient.Context.ToList();
        }
        public int CountInitInfo(uint cookie, byte version)
        {
            return _redisClient.Context.Count(k =>
                         k.Cookie == cookie
                         && k.Version == version);
        }

        public List<Aggregate.Redis.NatAddressInfo> GetInitInfos(Guid serverId, uint cookie)
        {
            return _redisClient.Context.Where(k =>
                 k.ServerID == serverId
                 && k.Cookie == cookie).ToList();
        }

        public void UpdateInitInfo(Aggregate.Redis.NatAddressInfo info)
        {
            _ = _redisClient.SetValueAsync(info);
        }

        public void RemoveInitInfo(Aggregate.Redis.NatAddressInfo info)
        {
            _redisClient.DeleteKeyValue(info);
        }

        public void UpdateNatFailInfo(Aggregate.Redis.Fail.NatFailInfo info)
        {
            _ = _natFailRedisClient.SetValueAsync(info);
        }

        public int GetNatFailInfo(Aggregate.Redis.Fail.NatFailInfo info)
        {
            return _natFailRedisClient.Context.Where(i => i.PublicIPAddress1.Equals(info.PublicIPAddress1) && i.PublicIPAddress2.Equals(info.PublicIPAddress2)).Count();
        }
    }
}