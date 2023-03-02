using System.Net;
using UniSpy.Server.Master.Aggregation.Redis;

namespace UniSpy.Server.Master.Abstraction.Interface
{
    public interface IStorageOperation
    {
        string GetGameSecretKey(string gameName);
        GameServerInfo GetServerInfo(IPEndPoint endPoint);
        void UpdateServerInfo(GameServerInfo info);
        void RemoveServerInfo(IPEndPoint endPoint);
    }
}