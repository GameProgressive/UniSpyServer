using System;
using GameSpyLib.Database.DatabaseModel.MySql;

namespace QueryReport.Entity.Structure.Group
{
    public class PeerRoom
    {
        public string RoomName;
        public int MaxPlayers;
        public int NumberPlayers;
        public int NumberWatingPlayers;
        public int NumberServers;
        public string OtherData;
        public string Password;
        public DateTime UpdateTime;

        public PeerRoom(Grouplist grouplist)
        {
            RoomName = grouplist.Name;
            NumberPlayers = grouplist.Numplayers;
            OtherData = grouplist.Other;
            Password = grouplist.Password.ToString();
        }
    }
}
