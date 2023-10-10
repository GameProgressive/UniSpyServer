using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.QueryReport.V1.Abstraction.Interface;
using System.Linq;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;
using System.Net;
using System.Collections.Generic;

namespace UniSpy.Server.QueryReport.V1.Application
{
    public class StorageOperation : IStorageOperation
    {
        private static GameServerCache.RedisClient _redisClient = new();
        public static IStorageOperation Persistance = new StorageOperation();
        public string GetGameSecretKey(string gameName)
        {
            using (var db = new UniSpyContext())
            {
                var result = from p in db.Games
                             where p.Gamename == gameName
                             select new { p.Secretkey };

                if (result.Count() != 1)
                {
                    throw new QueryReport.Exception($"No secret key found for game:{gameName}");
                }
                return result.First().Secretkey;
            }
        }
        public List<GameServerCache> GetServersInfo(string gameName)
        {
            var result = _redisClient.Context.Where(s => s.GameName == gameName).ToList();
            return result;
        }
        public GameServerCache GetServerInfo(IPEndPoint endPoint)
        {
            var result = _redisClient.Context.Where(s => s.HostIPAddress == endPoint.Address && s.HostPort == endPoint.Port);
            if (result.Count() != 1)
            {
                throw new QueryReport.Exception("Multiple server found in redis.");
            }
            return result.First();
        }
        public void UpdateServerInfo(GameServerCache info)
        {
            _ = _redisClient.SetValueAsync(info);
        }
        public void RemoveServerInfo(IPEndPoint endPoint)
        {
            var result = _redisClient.Context.Where(s => s.HostIPAddress == endPoint.Address && s.HostPort == endPoint.Port);
            foreach (var info in result)
            {
                _redisClient.DeleteKeyValue(info);
            }
        }
    }
}