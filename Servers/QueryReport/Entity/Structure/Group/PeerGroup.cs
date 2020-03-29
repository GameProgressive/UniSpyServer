using System;
using System.Collections.Generic;
using GameSpyLib.Database.DatabaseModel.MySql;
using System.Linq;

namespace QueryReport.Entity.Structure.Group
{

    public class PeerGroup
    {
        public string GameName { get;  set; }
        public int GameID { get;  set; }
        public List<PeerRoom> PeerRooms { get; set; }

        public PeerGroup()
        {
            PeerRooms = new List<PeerRoom>();
        }
        public PeerGroup(Grouplist grouplist,string gameName)
        {
            PeerRooms = new List<PeerRoom>();
            GameName = gameName;
            GameID = grouplist.Gameid;
        }
    }
}
