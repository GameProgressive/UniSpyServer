using System.Collections.Generic;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;

namespace UniSpy.Server.QueryReport.V2.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<GameServerInfo> GetServerInfos(uint instantKey);
        void UpdateGameServer(GameServerInfo info);
        void RemoveGameServer(GameServerInfo info);
    }
}