using System;
using System.Collections.Generic;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace QueryReport.Entity.Structure.Redis
{
    public class PeerRoomInfo
    {
        public static readonly List<string> GameSpyStandardKey =
            new List<string>
            {
                "groupid","hostname","numwaiting",
                "maxwaiting","maxplayers","numservers",
                "numplayers","password","numGames",
                "numplaying"
            };

        public Dictionary<string, string> KeyValue { get; protected set; }

        public DateTime UpdateTime;

        public PeerRoomInfo(Grouplist grouplist)
        {
            KeyValue = new Dictionary<string, string>();

            KeyValue.Add("groupid", grouplist.Groupid.ToString());
            KeyValue.Add("hostname", grouplist.Roomname);
            KeyValue.Add("numwaiting", "0");
            KeyValue.Add("maxwaiting", "200");
            KeyValue.Add("maxplayers", "200");
            KeyValue.Add("numservers", "0");
            KeyValue.Add("numplayers", "0");
            KeyValue.Add("password", "");
            KeyValue.Add("numGames", "0");
            KeyValue.Add("numplaying", "0");

            UpdateTime = DateTime.Now;

        }
        public PeerRoomInfo()
        {
            KeyValue = new Dictionary<string, string>();
        }
    }
}
