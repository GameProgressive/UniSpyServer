using System.Collections.Generic;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.GameServer;

namespace UniSpyServer.Servers.QueryReport.V2.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<GameServerInfo> GetServerInfos(uint instantKey);
        void UpdateGameServer(GameServerInfo info);
        void RemoveGameServer(GameServerInfo info);
    }
}