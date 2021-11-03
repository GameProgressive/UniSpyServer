using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis
{

    public class PeerGroupInfo
    {
        public string GameName { get; protected set; }
        public uint GameID { get; protected set; }
        public List<PeerRoomInfo> PeerRooms { get; protected set; }

        public PeerGroupInfo()
        {
            PeerRooms = new List<PeerRoomInfo>();
        }

        public PeerGroupInfo(Grouplist grouplist, string gameName)
        {
            PeerRooms = new List<PeerRoomInfo>();
            GameName = gameName;
            GameID = grouplist.Gameid;
        }
    }
}
