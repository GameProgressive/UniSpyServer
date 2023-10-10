using System;
using System.Linq;

namespace UniSpy.Server.Chat.Aggregate.Redis
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
}