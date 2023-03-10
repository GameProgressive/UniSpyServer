using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo
{
    public sealed class ChannelUser
    {
        /// <summary>
        /// Indicate whether this client is shared from redis channel
        /// </summary>
        [JsonIgnore]
        public bool IsRemoteUser => Info.IsRemoteClient;
        public bool IsVoiceable { get; set; }
        public bool IsChannelCreator { get; set; }
        public bool IsChannelOperator { get; set; }
        /// <summary>
        /// The remote ip end point of this user
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public IPEndPoint RemoteIPEndPoint => ClientRef.Connection.RemoteIPEndPoint;
        [JsonIgnore]
        public IChatClient ClientRef { get; private set; }
        [JsonIgnore]
        public ClientInfo Info => ClientRef.Info;
        [JsonIgnore]
        public IConnection Connection => ClientRef.Connection;
        /// <summary>
        /// The user key values storage
        /// </summary>
        public KeyValueManager KeyValues { get; private set; } = new KeyValueManager();
        [JsonIgnore]
        public Channel BelongedChannel { get; private set; }
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
        public ChannelUser(IChatClient client, Channel channel)
        {
            ClientRef = client;
            BelongedChannel = channel;
        }

        public void SetDefaultProperties(bool isCreator = false, bool isOperator = false)
        {
            IsVoiceable = true;
            IsChannelCreator = isCreator;
            IsChannelOperator = isOperator;
            // KeyValues.Update(new KeyValuePair<string, string>("username", Info.UserName));
            // KeyValues.Update(new KeyValuePair<string, string>("b_flags", "sh"));
        }
    }
}
