using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Extensions;
using System.Collections.Generic;
using QueryReport.Handler.SystemHandler.Redis;

namespace QueryReport.Entity.Structure.Group
{

    public class PeerGroupInfo
    {
        public string GameName { get; protected set; }
        public uint GameID { get; protected set; }
        public List<PeerRoomInfo> PeerRooms { get; protected set; }
        public static PeerGroupInfoRedisOperator RedisOperator { get; protected set; }

        static PeerGroupInfo()
        {
            RedisOperator = new PeerGroupInfoRedisOperator();
        }

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
