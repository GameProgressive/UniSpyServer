using System.Net;
using UniSpy.Server.QueryReport.V1.Aggregation.Redis;

namespace UniSpy.Server.QueryReport.V1.Abstraction.Interface
{
    public interface IStorageOperation
    {
        string GetGameSecretKey(string gameName);
        GameServerInfo GetServerInfo(IPEndPoint endPoint);
        void UpdateServerInfo(GameServerInfo info);
        void RemoveServerInfo(IPEndPoint endPoint);
    }
}