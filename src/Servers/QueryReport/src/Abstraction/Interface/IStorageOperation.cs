using System.Collections.Generic;
using UniSpy.Server.QueryReport.Aggregate.Redis.GameServer;

namespace UniSpy.Server.QueryReport.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<GameServerInfo> GetServerInfos(uint instantKey);
        void UpdateGameServer(GameServerInfo info);
        void RemoveGameServer(GameServerInfo info);
    }
}