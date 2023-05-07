using System.Collections.Generic;
using System.Net;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<GameServerInfo> GetServerInfos(uint instantKey);
        void UpdateGameServer(GameServerInfo info);
        void RemoveGameServer(GameServerInfo info);
        GameServerInfo GetGameServerInfo(IPEndPoint end);
        List<GameServerInfo> GetGameServerInfos(string gameName);
        Dictionary<string, List<Core.Database.DatabaseModel.Grouplist>> GetAllGroupList();
        public void PublishClientMessage(ClientMessageRequest message);

    }
}