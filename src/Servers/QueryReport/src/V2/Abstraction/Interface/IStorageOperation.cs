using System.Collections.Generic;
using System.Net;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<GameServerCache> GetServerInfos(uint instantKey);
        void UpdateGameServer(GameServerCache info);
        void RemoveGameServer(GameServerCache info);
        GameServerCache GetGameServerInfo(IPEndPoint end);
        List<GameServerCache> GetGameServerInfos(string gameName);
        public void PublishClientMessage(ClientMessageRequest message);

    }
}