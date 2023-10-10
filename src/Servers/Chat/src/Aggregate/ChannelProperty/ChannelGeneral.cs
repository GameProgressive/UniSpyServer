using System;
using System.Linq;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate.Redis;

namespace UniSpy.Server.Chat.Aggregate
{
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
        /// <summary>
        /// Join handler creates Broker and stored on local
        /// </summary>
        [JsonIgnore]
        public ChannelMessageBroker Broker
        {
            get
            {
                if (_broker is null)
                {
                    MessageBrokers.TryGetValue(Name, out _broker);
                }
                return _broker;
            }
        }
        [JsonIgnore]
        private ChannelMessageBroker _broker;
        public int? GroupId { get; private set; }
        public string RoomName { get; private set; }
        public bool IsValidPeerRoom => GroupId is not null && RoomName is not null;
        public string PreviousJoinedChannel { get; private set; }
        public Channel() { }
        public Channel(string name, IShareClient client, string password = null)
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
            if (Chat.Application.StorageOperation.Persistance.PeerGroupList.ContainsKey(GameName))
            {
                var grouplist = Chat.Application.StorageOperation.Persistance.PeerGroupList[GameName];
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
