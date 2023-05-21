using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.LinqToRedis;
using UniSpy.Server.Core.Config;
using UniSpy.Server.Core.Extension.Redis;

namespace UniSpy.Server.QueryReport.Aggregate.Redis.Channel
{

    public static class PeerRoom
    {
        /// <summary>
        /// When game connects to the server, the player will enter the default channel for communicating with other players.
        /// </summary>
        public const string TitleRoomPrefix = "#GSP";
        /// <summary>
        /// When a player creates their own game and is waiting for others to join they are placed in a separate chat room called the "staging room"
        /// Staging rooms have two title seperator like #GSP!xxxx!xxxx
        /// </summary>
        public const string StagingRoomPrefix = "#GSP";
        /// <summary>
        /// group rooms is used split the list of games into categories (by gametype, skill, region, etc.). In this case, when entering the title room, the user would get a list of group rooms instead of a list of games
        /// Group room have one title seperator like #GPG!xxxxxx
        /// </summary>
        public const string GroupRoomPrefix = "#GPG";
        public const char TitleSeperator = '!';
        /// <summary>
        /// Group room #GPG!622
        /// Staging room #GSP!worms3!Ml4lz344lM
        /// Normal room #islanbul
        /// </summary>
        /// <param name="channelName"></param>
        /// <returns></returns>
        public static PeerRoomType GetRoomType(string channelName)
        {
            if (IsGroupRoom(channelName))
            {
                // var c = StorageOperation.Persistance.IsPeerLobby(channelName);

                return PeerRoomType.Group;
            }
            else if (IsStagingRoom(channelName))
            {
                return PeerRoomType.Staging;
            }
            else if (IsTitleRoom(channelName))
            {
                return PeerRoomType.Title;
            }
            else
            {
                return PeerRoomType.Normal;
            }
        }
        private static bool IsStagingRoom(string channelName)
        {
            var a = channelName.Count(c => c == TitleSeperator) == 2 ? true : false;
            var b = channelName.StartsWith(StagingRoomPrefix, StringComparison.CurrentCultureIgnoreCase) ? true : false;
            return a && b;
        }
        private static bool IsTitleRoom(string channelName)
        {
            var a = channelName.Count(c => c == TitleSeperator) == 1 ? true : false;
            var b = channelName.StartsWith(TitleRoomPrefix, StringComparison.CurrentCultureIgnoreCase) ? true : false;
            return a && b;
        }
        private static bool IsGroupRoom(string channelName)
        {

            var a = channelName.Count(c => c == TitleSeperator) == 1 ? true : false;
            var b = channelName.StartsWith(GroupRoomPrefix, StringComparison.CurrentCultureIgnoreCase) ? true : false;
            return a && b;
        }
    }
    public enum PeerRoomType
    {
        /// <summary>
        /// The main room for a game.
        /// </summary>
        Title,
        /// <summary>
        /// A room where players meet before starting a game.
        /// </summary>
        Staging,
        /// <summary>
        /// A room which is, in general, for a particular type of gameplay (team, dm, etc.).
        /// </summary>
        Group,
        /// <summary>
        /// The normal room
        /// </summary>
        Normal
    }
    /// <summary>
    /// The channel info that stored in redis
    /// </summary>
    public record ChannelInfo : UniSpy.Server.Core.Abstraction.BaseClass.RedisKeyValueObject
    {
        [RedisKey]
        public Guid? ServerId { get; set; }
        [RedisKey]
        public string Name { get; set; }
        [RedisKey]
        public string PreviousJoinedChannel { get; set; }
        public PeerRoomType? RoomType { get; set; }
        public int? GroupId { get; set; }
        public string RoomName { get; set; }
        public string GameName { get; set; }
        public int MaxNumberUser { get; set; }
        public DateTime CreateTime { get; set; }
        public List<string> Users { get; set; }
        public ChannelInfo() : base(RedisDbNumber.ChatChannel, TimeSpan.FromHours(1)) { }
    }
    public class RedisClient : UniSpy.Server.Core.Abstraction.BaseClass.RedisClient<ChannelInfo>
    {
        public RedisClient() { }
    }
}