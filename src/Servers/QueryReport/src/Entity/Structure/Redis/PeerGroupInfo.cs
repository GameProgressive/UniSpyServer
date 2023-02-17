using System;
using System.Collections.Generic;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.Core.Extensions;

namespace UniSpy.Server.QueryReport.V2.Entity.Structure.Redis.PeerGroup
{
    public record PeerGroupInfo : RedisKeyValueObject
    {
        [RedisKey]
        public string GameName { get; init; }
        public int GameID { get; init; }
        public List<PeerRoomInfo> PeerRooms { get; init; }

        public PeerGroupInfo() : base(expireTime: null)
        {
        }
    }
    public class PeerRoomInfo
    {
        public int GroupId => (int)KeyValues["groupid"];
        public string HostName => (string)KeyValues["hostname"];
        public int NumberOfWaitingPlayers => (int)KeyValues["numwaiting"];
        public int MaxNumberOfWaitingPlayers => (int)KeyValues["maxwaiting"];
        public int NumberOfServers => (int)KeyValues["numservers"];
        public int NumberOfPlayers => (int)KeyValues["numplayers"];
        public int MaxNumberOfPlayers => (int)KeyValues["maxplayers"];
        public string Password => (string)KeyValues["password"];
        public int NumberOfGames => (int)KeyValues["numgames"];
        public int NumberOfPlayingPlayers => (int)KeyValues["numplaying"];
        public Dictionary<string, object> KeyValues { get; private set; }
        public DateTime UpdateTime;

        public PeerRoomInfo(Grouplist groupList)
        {
            UpdateTime = DateTime.Now;

            KeyValues = new Dictionary<string, object>();
            // this is the default value that every game needed
            KeyValues.Add("groupid", groupList.Groupid);
            KeyValues.Add("hostname", groupList.Roomname);
            KeyValues.Add("numwaiting", 0);
            KeyValues.Add("maxwaiting", 200);
            KeyValues.Add("numservers", 0);
            KeyValues.Add("numplayers", 0);
            KeyValues.Add("maxplayers", 200);
            KeyValues.Add("password", "");
            KeyValues.Add("numgames", 0);
            KeyValues.Add("numplaying", 0);
        }

        public PeerRoomInfo()
        {
        }
    }
    public class RedisClient : LinqToRedis.RedisClient<PeerGroupInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.RedisConnection, (int)DbNumber.PeerGroup)
        {
        }
    }
}