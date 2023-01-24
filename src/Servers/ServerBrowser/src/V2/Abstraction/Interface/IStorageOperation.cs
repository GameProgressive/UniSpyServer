using System.Linq;
using System.Collections.Generic;
using System.Net;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Redis.PeerGroup;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using UniSpyServer.Servers.QueryReport.V2.Entity.Structure.Request;

namespace UniSpyServer.Servers.ServerBrowser.Abstraction.Interface
{
    public interface IStorageOperation
    {
        GameServerInfo GetGameServerInfo(IPEndPoint end);
        List<GameServerInfo> GetGameServerInfos(string gameName);
        PeerGroupInfo GetPeerGroupInfo(string gameName);
        IQueryable<Grouplist> GetGroupList(string gameName);
        public void PublishClientMessage(ClientMessageRequest message);
    }
}