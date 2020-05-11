using GameSpyLib.Database.DatabaseModel.MySql;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Group
{
    public class PeerRoom
    {
        public static readonly List<string> GameSpyStandardKey =
            new List<string>
            {
                "groupid","hostname","numwaiting","maxwaiting","maxplayers","numservers",
                "numplayers","password","numGames","numplaying"
            };

        public Dictionary<string, string> KeyValue { get; protected set; }

        public DateTime UpdateTime;

        public PeerRoom(Grouplist grouplist)
        {
            KeyValue = new Dictionary<string, string>();

            KeyValue.Add("groupid", grouplist.Id.ToString());
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
        public PeerRoom()
        {
            KeyValue = new Dictionary<string, string>();
        }
    }
}
