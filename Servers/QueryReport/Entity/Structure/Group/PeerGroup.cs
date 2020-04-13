using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Extensions;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Group
{

    public class PeerGroup
    {
        public string GameName { get; set; }
        public int GameID { get; set; }
        public List<PeerRoom> PeerRooms { get; set; }

        public static List<string> PeerGroupKeyList
        {
            get
            { return GetAllKeys(); }
        }

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

        public static List<string> GetAllKeys()
        {
           return RedisExtensions.GetAllKeys(RedisDBNumber.PeerGroup);
        }

        public static PeerGroup GetGroupsList(string gameName)
        {
            return RedisExtensions.SerilizeGet<PeerGroup>(gameName, (int)RedisDBNumber.PeerGroup);
        }

        public static bool SetGroupList(string gameName, PeerGroup group)
        {
            return RedisExtensions.SerializeSet(gameName, group, (int)RedisDBNumber.PeerGroup);
        }
    }
}
