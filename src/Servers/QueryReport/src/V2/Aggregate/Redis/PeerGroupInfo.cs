using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Extension.Redis;

//!fix move this to root namespace 
namespace UniSpy.Server.QueryReport.V2.Aggregate.Redis.PeerGroup
{
    public record PeerRoomInfo : RedisKeyValueObject
    {
        [RedisKey]
        [JsonProperty]
        public string GameName { get; private set; }
        [RedisKey]
        [JsonProperty]
        public int? GroupId { get; private set; }
        [RedisKey]
        [JsonProperty]
        public string RoomName { get; private set; }
        [JsonIgnore]
        public int NumberOfWaitingPlayers
        {
            get => int.Parse(KeyValues["numwaiting"]);
            set => KeyValues["numwaiting"] = value.ToString();
        }
        [JsonIgnore]
        public int MaxNumberOfWaitingPlayers
        {
            get => int.Parse(KeyValues["maxwaiting"]);
            set => KeyValues["maxwaiting"] = value.ToString();
        }
        [JsonIgnore]
        public int NumberOfServers
        {
            get => int.Parse(KeyValues["numservers"]);
            set => KeyValues["numservers"] = value.ToString();
        }
        [JsonIgnore]
        public int NumberOfPlayers
        {
            get => int.Parse(KeyValues["numplayers"]);
            set => KeyValues["numplayers"] = value.ToString();
        }
        [JsonIgnore]
        public int MaxNumberOfPlayers
        {
            get => int.Parse(KeyValues["maxplayers"]);
            set => KeyValues["maxplayers"] = value.ToString();
        }
        [JsonIgnore]
        public string Password => (string)KeyValues["password"];
        [JsonIgnore]
        public int NumberOfGames
        {
            get => int.Parse(KeyValues["numgames"]);
            set => KeyValues["numgames"] = value.ToString();
        }
        [JsonIgnore]
        public int NumberOfPlayingPlayers
        {
            get => int.Parse(KeyValues["numplaying"]);
            set => KeyValues["numplaying"] = value.ToString();
        }
        public Dictionary<string, string> KeyValues { get; private set; } = new Dictionary<string, string>();
        public DateTime UpdateTime;
        public PeerRoomInfo() : base(expireTime: null) { }
        public PeerRoomInfo(string gameName, int groupId, string roomName) : base(expireTime: null)
        {
            UpdateTime = DateTime.Now;
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
    internal class RedisClient : LinqToRedis.RedisClient<PeerRoomInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)RedisDbNumber.PeerGroup)
        {
        }
    }
}