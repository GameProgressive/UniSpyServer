using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.Database.DatabaseModel;
using UniSpy.Server.QueryReport.Aggregate.Redis.Channel;

namespace UniSpy.Server.QueryReport.Application
{
    public class StorageOperation
    {
        /// <summary>
        /// The peer group list in memory
        /// </summary>
        public static readonly Dictionary<string, List<Grouplist>> PeerGroupList = QueryReport.V2.Application.StorageOperation.Persistance.GetAllGroupList();
        public static QueryReport.Aggregate.Redis.Channel.RedisClient _chatChannelRedisClient = new QueryReport.Aggregate.Redis.Channel.RedisClient();
        public static bool UpdateChannel(ChannelInfo channel) => _chatChannelRedisClient.SetValue(channel);
        public static void RemoveChannel(ChannelInfo channel) => _chatChannelRedisClient.DeleteKeyValue(channel);
        public static List<ChannelInfo> GetPeerStagingChannel(string gameName, int groupId)
        {
            var stagingName = $"{PeerRoom.StagingRoomPrefix}!{gameName}!*";
            var groupName = $"{PeerRoom.GroupRoomPrefix}!{groupId}";
            var stagingRooms = QueryReport.Application.StorageOperation._chatChannelRedisClient.Context.Where(c => c.Name == stagingName && c.PreviousJoinedChannel == groupName).ToList();
            return stagingRooms;
        }
        public static List<ChannelInfo> GetPeerGroupChannel(int groupId)
        {
            var groupName = $"{PeerRoom.GroupRoomPrefix}!{groupId}";
            var groupRooms = QueryReport.Application.StorageOperation._chatChannelRedisClient.Context.Where(c => c.Name == groupName).ToList();
            return groupRooms;
        }
    }
}