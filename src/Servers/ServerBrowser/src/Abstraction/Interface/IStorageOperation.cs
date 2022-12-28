using System.Linq;
using System.Collections.Generic;
using System.Net;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.PeerGroup;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.ServerBrowser.Abstraction.Interface
{
    public interface IStorageOperation
    {
        GameServerInfo GetGameServerInfo(IPAddress address, ushort queryReportPort);
        List<GameServerInfo> GetGameServerInfos(string gameName);
        PeerGroupInfo GetPeerGroupInfo(string gameName);
        IQueryable<Grouplist> GetGroupList(string gameName);
    }
}