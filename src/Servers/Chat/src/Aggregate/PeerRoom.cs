using System.Linq;
using UniSpy.Server.Chat.Application;

namespace UniSpy.Server.Chat.Aggregate
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

        public static PeerRoomType GetRoomType(string channelName)
        {
            var c = StorageOperation.Persistance.IsPeerLobby(channelName);

            if (IsGroupRoom(channelName) || c)
            {
                return PeerRoomType.Group;
            }
            else if (IsStagingRoom(channelName) || c)
            {
                return PeerRoomType.Staging;
            }
            else if (IsTitleRoom(channelName) || c)
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
            var b = channelName.StartsWith(StagingRoomPrefix) ? true : false;
            return a && b;
        }
        private static bool IsTitleRoom(string channelName)
        {
            var a = channelName.Count(c => c == TitleSeperator) == 1 ? true : false;
            var b = channelName.StartsWith(TitleRoomPrefix) ? true : false;
            return a && b;
        }
        private static bool IsGroupRoom(string channelName)
        {

            var a = channelName.Count(c => c == TitleSeperator) == 1 ? true : false;
            var b = channelName.StartsWith(GroupRoomPrefix) ? true : false;
            return a && b;
        }
    }
    public enum PeerRoomType
    {
        /// <summary>
        /// the first channel that a connected user joined at first time
        /// </summary>
        Title,
        /// <summary>
        /// User created room for gaming
        /// </summary>
        Staging,
        /// <summary>
        /// User created room which can be seperated by categories
        /// </summary>
        Group,
        /// <summary>
        /// Testing room
        /// </summary>
        Normal
    }
}