using System.Collections.Generic;
using System.Net;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.PeerGroup;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.ServerBrowser.V2.Abstraction.Interface
{
    public interface IStorageOperation
    {
        GameServerInfo GetGameServerInfo(IPEndPoint end);
        List<GameServerInfo> GetGameServerInfos(string gameName);
        PeerGroupInfo GetPeerGroupInfo(string gameName);
        List<Grouplist> GetGroupList(string gameName);
        Dictionary<string, List<Grouplist>> GetAllGroupList();
        public void PublishClientMessage(ClientMessageRequest message);
    }
}