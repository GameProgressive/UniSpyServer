using System;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.Chat.Aggregate
{
    public sealed class ChannelUser
    {
        [JsonProperty]
        public Guid? ServerId { get; private set; }
        /// <summary>
        /// Indicate whether this client is shared from redis channel
        /// </summary>
        public bool IsVoiceable { get; set; } = true;
        public bool IsChannelCreator { get; set; }
        public bool IsChannelOperator { get; set; }
        public bool IsRemoteClient => Client.IsRemoteClient;
        /// <summary>
        /// The remote ip end point of this user
        /// </summary>
        [JsonProperty]
        [JsonConverter(typeof(IPEndPointConverter))]
        public IPEndPoint RemoteIPEndPoint { get; private set; }
        /// <summary>
        /// The client reference
        /// </summary>
        [JsonIgnore]
        public IShareClient Client
        {
            get
            {
                if (_client is not null)
                {
                    return _client;
                }
                else
                {
                    var client = ClientManager.GetClient(RemoteIPEndPoint);
                    if (client is null)
                    {
                        throw new UniSpy.Exception("the client is not on local server, please check logic");
                    }
                    return (IShareClient)client;
                }
            }
        }
        [JsonIgnore]
        private IShareClient _client;
        /// <summary>
        /// The user key values storage
        /// </summary>
        public KeyValueManager KeyValues { get; private set; } = new KeyValueManager();
        /// <summary>
        /// The channel where user current in.
        /// </summary>
        [JsonIgnore]
        public Channel Channel { get; private set; }
        [JsonIgnore]
        public string Modes
        {
            get
            {
                var buffer = new StringBuilder();

                if (IsChannelOperator)
                {
                    buffer.Append("@");
                }

                if (IsVoiceable)
                {
                    buffer.Append("+");
                }

                return buffer.ToString();
            }
        }
        public ChannelUser() { }
        public ChannelUser(IShareClient client, Channel channel)
        {
            _client = client;
            Channel = channel;
            ServerId = client.Server.Id;
            RemoteIPEndPoint = client.Connection.RemoteIPEndPoint;
        }
    }
}
