using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Aggregate.Redis;
using UniSpy.Server.QueryReport.Abstraction.Interface;

namespace UniSpy.Server.QueryReport.Application
{
    public class StorageOperation : IStorageOperation
    {
        public static IStorageOperation Persistance = new StorageOperation();
        public List<Channel> GetPeerStagingChannel(string gameName, int groupId)
        {
            var stagingName = $"{PeerRoom.StagingRoomPrefix}!{gameName}!*";
            var groupName = $"{PeerRoom.GroupRoomPrefix}!{groupId}";
            var stagingRooms = Chat.Application.StorageOperation.Persistance.ChannelCacheClient.Context
            .Where(c => c.ChannelName == stagingName).ToList()
            .Where(c => c.Channel.PreviousJoinedChannel == groupName).Select(c => c.Channel).ToList();
            return stagingRooms;
        }
        public List<Channel> GetPeerGroupChannel(int groupId)
        {
            var groupName = $"{PeerRoom.GroupRoomPrefix}!{groupId}";
            var groupRooms = Chat.Application.StorageOperation.Persistance.ChannelCacheClient.Context.Where(c => c.ChannelName == groupName).ToList().Select(c => c.Channel).ToList();
            return groupRooms;
        }
    }
}