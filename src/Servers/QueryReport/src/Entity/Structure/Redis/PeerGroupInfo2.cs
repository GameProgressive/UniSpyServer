using System;
using System.Collections.Generic;
using UniSpyServer.LinqToRedis;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.PeerGroup
{
    public record PeerGroupInfo2 : RedisKeyValueObject
    {
        [RedisKey]
        public string GameName { get; set; }
        public uint GameID { get; protected set; }
        public List<PeerRoomInfo> PeerRooms { get; protected set; }

        public PeerGroupInfo2() : base(expireTime: null)
        {
        }
    }
    public class PeerRoomInfo
    {
        public uint GroupID { get; set; }
        public string HostName { get; set; }
        public uint NumberOfWaitingPlayers { get; set; }
        public uint MaxNumberOfWaitingPlayers { get; set; }
        public uint NumberOfServers { get; set; }
        public uint NumberOfPlayers { get; set; }
        public uint MaxNumberOfPlayers { get; set; }
        public string Password { get; set; }
        public uint NumberOfGames { get; set; }
        public uint NumberOfPlayingPlayers { get; set; }
        public DateTime UpdateTime;

        public PeerRoomInfo(Grouplist groupList)
        {
            UpdateTime = DateTime.Now;
            GroupID = groupList.GroupID;
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
        public string GetValuebyGameSpyDefinedName(string key)
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
    public class RedisClient : LinqToRedis.RedisClient<PeerGroupInfo2>
    {
        public RedisClient() : base(ConfigManager.Config.Redis.ConnectionString, (int)DbNumber.PeerGroup)
        {
        }
    }
}