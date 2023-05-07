using System;
using System.Collections.Generic;


//!fix move this to root namespace 
namespace UniSpy.Server.QueryReport.Aggregate.Redis.PeerGroup
{
    public record PeerRoomInfo
    {
        public Guid? ServerId { get; set; }
        public string GameName { get; private set; }
        public int? GroupId { get; private set; }
        public string RoomName { get; private set; }
        public int NumberOfWaitingPlayers
        {
            get => int.Parse(KeyValues["numwaiting"]);
            set => KeyValues["numwaiting"] = value.ToString();
        }
        public int MaxNumberOfWaitingPlayers
        {
            get => int.Parse(KeyValues["maxwaiting"]);
            set => KeyValues["maxwaiting"] = value.ToString();
        }
        public int NumberOfServers
        {
            get => int.Parse(KeyValues["numservers"]);
            set => KeyValues["numservers"] = value.ToString();
        }

        public int MaxNumberOfPlayers
        {
            get => int.Parse(KeyValues["maxplayers"]);
            set => KeyValues["maxplayers"] = value.ToString();
        }
        public string Password => (string)KeyValues["password"];
        public int NumberOfGames
        {
            get => int.Parse(KeyValues["numgames"]);
            set => KeyValues["numgames"] = value.ToString();
        }
        public int NumberOfPlayers
        {
            get => int.Parse(KeyValues["numplayers"]);
            set => KeyValues["numplayers"] = value.ToString();
        }
        public int NumberOfPlayingPlayers
        {
            get => int.Parse(KeyValues["numplaying"]);
            set => KeyValues["numplaying"] = value.ToString();
        }
        public Dictionary<string, string> KeyValues { get; private set; } = new Dictionary<string, string>();
        public PeerRoomInfo(string gameName, int groupId, string roomName)
        {
            // this is the default value that every game needed
            GameName = gameName;
            GroupId = groupId;
            RoomName = roomName;
            KeyValues.Add("groupid", groupId.ToString());
            KeyValues.Add("hostname", roomName);
            KeyValues.Add("numwaiting", "0");
            KeyValues.Add("maxwaiting", "200");
            KeyValues.Add("numservers", "0");
            KeyValues.Add("numplayers", "0");
            KeyValues.Add("maxplayers", "200");
            KeyValues.Add("password", "");
            KeyValues.Add("numgames", "0");
            KeyValues.Add("numplaying", "0");
        }
    }
}