using System;
using System.Linq;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Aggregate
{
    public sealed partial record Channel : LinqToRedis.RedisKeyValueObject
    {
        [JsonProperty]
        public Guid ServerId { get; private set; }
        [JsonProperty]
        public string GameName { get; private set; }
        /// <summary>
        /// Channel name
        /// </summary>
        /// <value></value>
        [LinqToRedis.RedisKey]
        [JsonProperty]
        public string Name { get; private set; }
        /// <summary>
        /// The maximum number of users that can be in the channel
        /// </summary>
        /// <value></value>
        [JsonProperty]
        public int MaxNumberUser { get; private set; } = 200;

        [JsonProperty]
        public DateTime CreateTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// Channel key values
        /// </summary>
        [JsonProperty]
        public KeyValueManager KeyValues { get; private set; } = new KeyValueManager();

        [JsonProperty]
        public PeerRoomType RoomType { get; private set; }
        [JsonProperty]
        public string Password { get; private set; }
        public string Topic { get; set; }
        [JsonIgnore]
        public Redis.ChatMessageChannel MessageBroker { get; private set; }
        /// <summary>
        /// Constructor for json deserialization
        /// </summary>
        public Channel() : base(expireTime: TimeSpan.FromHours(1)) { }
        public Channel(string name, IChatClient client, string password = null) : base(expireTime: TimeSpan.FromHours(1))
        {
            ServerId = client.Server.Id;
            Name = name;
            Password = password;
            RoomType = PeerRoom.GetRoomType(Name);
            GameName = client.Info.GameName;
            switch (RoomType)
            {
                case PeerRoomType.Normal:
                case PeerRoomType.Staging:
                    // user created room
                    var user = AddUser(client, password, true, true);
                    _creatorNickName = user.Info.NickName;
                    break;
                case PeerRoomType.Title:
                case PeerRoomType.Group:
                    // official room can not create by user
                    AddUser(client, password);
                    break;
            }
            MessageBroker = new Redis.ChatMessageChannel(Name);
            MessageBroker.StartSubscribe();
        }
    }
}
