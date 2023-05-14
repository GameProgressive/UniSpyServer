using System;
using System.Linq;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.QueryReport.Aggregate.Redis.Channel;

namespace UniSpy.Server.Chat.Aggregate
{
    public sealed partial class Channel
    {
        public Guid ServerId { get; private set; }
        public string GameName { get; private set; }
        /// <summary>
        /// Channel name
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// The maximum number of users that can be in the channel
        /// </summary>
        public int MaxNumberUser { get; private set; } = 200;
        public DateTime CreateTime { get; private set; } = DateTime.Now;
        /// <summary>
        /// Channel key values
        /// </summary>
        public KeyValueManager KeyValues { get; private set; } = new KeyValueManager();
        public PeerRoomType? RoomType { get; private set; }
        public string Password { get; private set; }
        public string Topic { get; set; }
        public Redis.ChatMessageChannel MessageBroker { get; private set; }
        public int? GroupId { get; private set; }
        public string RoomName { get; private set; }
        public bool IsValidPeerRoom => GroupId is not null && RoomName is not null;
        public string PreviousJoinedChannel { get; private set; }
        public Channel(string name, IChatClient client, string password = null)
        {
            ServerId = client.Server.Id;
            Name = name;
            Password = password;
            RoomType = PeerRoom.GetRoomType(Name);
            GameName = client.Info.GameName;
            PreviousJoinedChannel = client.Info.PreviousJoinedChannel;
            switch (RoomType)
            {
                case PeerRoomType.Group:
                    GetGroupId();
                    GetPeerRoomName();
                    break;
                case PeerRoomType.Staging:
                    GetStagingRoomName();
                    break;
                case PeerRoomType.Title:
                    GetTitileRoomName();
                    break;
            }
            MessageBroker = new Redis.ChatMessageChannel(Name);
            MessageBroker.Subscribe();
        }
        private void GetGroupId()
        {
            var groupIdStr = Name.Split("!", StringSplitOptions.RemoveEmptyEntries)[1];
            if (!int.TryParse(groupIdStr, out var groupId))
            {
                throw new Chat.Exception("Peer room group id is incorrect");
            }
            GroupId = groupId;
        }
        private void GetPeerRoomName()
        {
            if (QueryReport.Application.StorageOperation.PeerGroupList.ContainsKey(GameName))
            {
                var grouplist = QueryReport.Application.StorageOperation.PeerGroupList[GameName];
                var room = grouplist.Where(g => g.Groupid == GroupId).FirstOrDefault();
                if (room is null)
                {
                    throw new Chat.Exception($"Invalid peer room: {Name}");
                }
                RoomName = room.Roomname;
            }
        }
        private void GetStagingRoomName()
        {
            RoomName = Name.Split('!', StringSplitOptions.RemoveEmptyEntries).Last();
        }
        private void GetTitileRoomName() => GetStagingRoomName();
    }
}
