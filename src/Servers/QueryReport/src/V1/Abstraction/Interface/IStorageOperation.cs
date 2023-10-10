using System.Collections.Generic;
using System.Net;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;

namespace UniSpy.Server.QueryReport.V1.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<GameServerCache> GetServersInfo(string gameName);
        string GetGameSecretKey(string gameName);
        GameServerCache GetServerInfo(IPEndPoint endPoint);
        void UpdateServerInfo(GameServerCache info);
        void RemoveServerInfo(IPEndPoint endPoint);
    }
}