using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.QueryReport.V1.Abstraction.Interface;
using System.Linq;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;
using System.Net;
using UniSpy.Server.QueryReport.Exception;

namespace UniSpy.Server.QueryReport.V1.Application
{
    public class StorageOperation : IStorageOperation
    {
        private static RedisClient _redisClient = new RedisClient();
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
                    throw new QRException($"No secret key found for game:{gameName}");
                }
                return result.First().Secretkey;
            }
        }

        public GameServerInfo GetServerInfo(IPEndPoint endPoint)
        {
            var result = _redisClient.Context.Where(s => s.HostIPAddress == endPoint.Address && s.QueryReportPort == endPoint.Port);
            if (result.Count() != 1)
            {
                throw new QRException("Multiple server found in redis.");
            }
            return result.First();
        }
        public void UpdateServerInfo(GameServerInfo info)
        {
            _ = _redisClient.SetValueAsync(info);
        }
        public void RemoveServerInfo(IPEndPoint endPoint)
        {
            var result = _redisClient.Context.Where(s => s.HostIPAddress == endPoint.Address && s.QueryReportPort == endPoint.Port);
            foreach (var info in result)
            {
                _redisClient.DeleteKeyValue(info);
            }
        }
    }
}