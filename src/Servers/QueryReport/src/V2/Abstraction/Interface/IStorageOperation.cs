using System.Collections.Generic;
using System.Net;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.GameServer;
using UniSpy.Server.QueryReport.V2.Aggregate.Redis.PeerGroup;
using UniSpy.Server.QueryReport.V2.Contract.Request;

namespace UniSpy.Server.QueryReport.V2.Abstraction.Interface
{
    public interface IStorageOperation
    {
        List<GameServerInfo> GetServerInfos(uint instantKey);
        void UpdateGameServer(GameServerInfo info);
        void RemoveGameServer(GameServerInfo info);
        void InitPeerRoomsInfo();
        List<PeerRoomInfo> GetPeerRoomsInfo(string gameName, int? groupId = null, string roomName = null);
        void UpdatePeerRoomInfo(PeerRoomInfo info);
        GameServerInfo GetGameServerInfo(IPEndPoint end);
        List<GameServerInfo> GetGameServerInfos(string gameName);
        // PeerGroupInfo GetPeerGroupInfo(string gameName);
        // List<Grouplist> GetGroupList(string gameName);
        Dictionary<string, List<Core.Database.DatabaseModel.Grouplist>> GetAllGroupList();
        List<Core.Database.DatabaseModel.Grouplist> GetGroupList(string gameName);
        public void PublishClientMessage(ClientMessageRequest message);

    }
}