using QueryReport.Handler.SystemHandler.Redis;
using System.Collections.Generic;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace QueryReport.Entity.Structure.Group
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
