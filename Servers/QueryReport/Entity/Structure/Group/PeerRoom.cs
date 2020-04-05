using GameSpyLib.Database.DatabaseModel.MySql;
using System;
using System.Collections.Generic;

namespace QueryReport.Entity.Structure.Group
{
    public class PeerRoom
    {
        public static readonly List<string> StandardKey =
            new List<string>
            {
                "groupid","hostname","numwaiting","maxwaiting","maxplayers","numservers",
                "numplayers","password","numGames","numplaying",
                "param"
            };

        public Dictionary<string, string> StandardKeyValue { get; protected set; }
        public Dictionary<string, string> CustomKeyValue { get; protected set; }
        ///// <summary>
        ///// The name of the group. 
        ///// </summary>
        //public string RoomName;
        ///// <summary>
        ///// The maximum number of players allowed in the group room. 
        ///// </summary>
        //public int MaxPlayers;
        ///// <summary>
        ///// The total number of players in all of this group's games. 
        ///// </summary>
        //public int NumberPlaying;

        ///// <summary>
        ///// The number of players in the group room. 
        ///// </summary>
        //public int NumberWating;

        ///// <summary>
        ///// The number of games currently in this group, either in staging or already running. 
        ///// </summary>
        //public int NumberGames;
        //public string OtherData;
        //public string Password;
        public DateTime UpdateTime;

        public PeerRoom(Grouplist grouplist)
        {
            StandardKeyValue = new Dictionary<string, string>();
            CustomKeyValue = new Dictionary<string, string>();

            StandardKeyValue.Add("groupid", grouplist.Groupid.ToString());
            StandardKeyValue.Add("hostname", grouplist.Roomname);
            StandardKeyValue.Add("numwaiting", "0");
            StandardKeyValue.Add("maxwaiting", "200");
            StandardKeyValue.Add("maxplayers", "200");
            StandardKeyValue.Add("numservers", "0");
            StandardKeyValue.Add("numplayers", "0");
            StandardKeyValue.Add("password", "");
            StandardKeyValue.Add("numGames", "0");
            StandardKeyValue.Add("numplaying", "0");

            UpdateTime = DateTime.Now;

        }
        public PeerRoom()
        {
            StandardKeyValue = new Dictionary<string, string>();
            CustomKeyValue = new Dictionary<string, string>();
        }
    }
}
