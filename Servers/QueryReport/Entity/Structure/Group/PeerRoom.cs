using System;
using System.Collections;
using GameSpyLib.Database.DatabaseModel.MySql;

namespace QueryReport.Entity.Structure.Group
{
    public class PeerRoom
    {
        /// <summary>
        /// The name of the group. 
        /// </summary>
        public string RoomName;
        /// <summary>
        /// The maximum number of players allowed in the group room. 
        /// </summary>
        public int MaxPlayers;
        /// <summary>
        /// The total number of players in all of this group's games. 
        /// </summary>
        public int NumberPlaying;

        /// <summary>
        /// The number of players in the group room. 
        /// </summary>
        public int NumberWating;

        /// <summary>
        /// The number of games currently in this group, either in staging or already running. 
        /// </summary>
        public int NumberGames;
        public string OtherData;
        public string Password;
        public DateTime UpdateTime;

        public PeerRoom(Grouplist grouplist)
        {
            RoomName = grouplist.Name;
            NumberPlaying = grouplist.Numplayers;
            OtherData = grouplist.Other;
            Password = grouplist.Password.ToString();
        }
        public PeerRoom()
        { }
    }
}
