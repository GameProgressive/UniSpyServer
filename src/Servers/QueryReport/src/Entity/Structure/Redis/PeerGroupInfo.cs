using System;
using System.Collections.Generic;
using UniSpyServer.LinqToRedis;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.PeerGroup
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
        public int GroupID { get; set; }
        public string HostName { get; set; }
        public int NumberOfWaitingPlayers { get; set; }
        public int MaxNumberOfWaitingPlayers { get; set; }
        public int NumberOfServers { get; set; }
        public int NumberOfPlayers { get; set; }
        public int MaxNumberOfPlayers { get; set; }
        public string Password { get; set; }
        public int NumberOfGames { get; set; }
        public int NumberOfPlayingPlayers { get; set; }
        public DateTime UpdateTime;

        public PeerRoomInfo(Grouplist groupList)
        {
            UpdateTime = DateTime.Now;
            GroupID = groupList.Groupid;
            HostName = groupList.Roomname;
            NumberOfWaitingPlayers = 0;
            MaxNumberOfWaitingPlayers = 200;
            MaxNumberOfPlayers = 200;
            NumberOfServers = 0;
            NumberOfPlayers = 0;
            Password = "";
            NumberOfGames = 0;
            NumberOfPlayingPlayers = 0;
        }

        public PeerRoomInfo()
        {
        }

        public string this[string key] => GetValuebyGameName(key);
        public string GetValuebyGameName(string key)
        {
            switch (key)
            {
                case "groupid":
                    return GroupID.ToString();
                case "hostname":
                    return HostName;
                case "numwaiting":
                    return NumberOfWaitingPlayers.ToString();
                case "maxwaiting":
                    return MaxNumberOfWaitingPlayers.ToString();
                case "maxplayers":
                    return MaxNumberOfPlayers.ToString();
                case "numservers":
                    return NumberOfServers.ToString();
                case "numplayers":
                    return NumberOfPlayers.ToString();
                case "password":
                    return Password;
                case "numGames":
                    return NumberOfGames.ToString();
                case "numplaying":
                    return NumberOfPlayingPlayers.ToString();
                default:
                    throw new ArgumentException("No matched Property found by giving key");

            }

        }
    }
    public class RedisClient : LinqToRedis.RedisClient<PeerGroupInfo>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.ConnectionString, (int)DbNumber.PeerGroup)
        {
        }
    }
}