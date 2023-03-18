using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.NatNegotiation.Abstraction.Interface;
using UniSpy.Server.NatNegotiation.Aggregate.Redis;

namespace UniSpy.Server.NatNegotiation.Application
{
    internal sealed class StorageOperation : IStorageOperation
    {
        /// <summary>
        /// natneg init information redis server.
        /// </summary>
        private static RedisClient _redisClient = new RedisClient();
        private static NatNegotiation.Aggregate.Redis.Fail.RedisClient _natFailRedisClient = new NatNegotiation.Aggregate.Redis.Fail.RedisClient();
        public static IStorageOperation Persistance = new StorageOperation();
        public int CountInitInfo(uint cookie, byte version)
        {
            return _redisClient.Context.Count(k =>
                         k.Cookie == cookie
                         && k.Version == version);
        }

        public List<NatAddressInfo> GetInitInfos(Guid serverId, uint cookie)
        {
            return _redisClient.Context.Where(k =>
                 k.ServerID == serverId
                 && k.Cookie == cookie).ToList();
        }

        public void UpdateInitInfo(NatAddressInfo info)
        {
            _ = _redisClient.SetValueAsync(info);
        }

        public void RemoveInitInfo(NatAddressInfo info)
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