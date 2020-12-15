using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Extensions;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Group
{

    public class PeerGroup
    {
        public string GameName { get; protected set; }
        public uint GameID { get; protected set; }
        public List<PeerRoom> PeerRooms { get; protected set; }

        public PeerGroup()
        {
            PeerRooms = new List<PeerRoom>();
        }
        public PeerGroup(Grouplist grouplist, string gameName)
        {
            PeerRooms = new List<PeerRoom>();
            GameName = gameName;
            GameID = grouplist.Gameid;
        }

        public static List<string> SearchPeerGroupKeys(string subKey)
        {
            return RedisExtensions.GetMatchedKeys(subKey, (int)RedisDBNumber.PeerGroup);
        }

        public static PeerGroup GetGroupsList(string gameName)
        {
            return RedisExtensions.GetValue<PeerGroup>(gameName, (int)RedisDBNumber.PeerGroup);
        }

        public static bool SetGroupList(string gameName, PeerGroup group)
        {
            return RedisExtensions.SetKeyValue(gameName, group, (int)RedisDBNumber.PeerGroup);
        }
    }
}
