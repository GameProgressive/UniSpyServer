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
        public Dictionary<string, string> UserKeyValue { get; private set; }
        [JsonIgnore]
        public string BFlags => $@"\{Info.UserName}\{UserKeyValue["b_flags"]}";
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
        public ChannelUser(IChatClient client)
        {
            ClientRef = client;
            UserKeyValue = new Dictionary<string, string>();
        }

        public void SetDefaultProperties(bool isCreator = false, bool isOperator = false)
        {
            IsVoiceable = true;
            IsChannelCreator = isCreator;
            IsChannelOperator = isOperator;
            UserKeyValue.Add("username", Info.UserName);
            // UserKeyValue.Add("b_flags", "");

            // if (isCreator)
            // {
            //     UserKeyValue.Add("b_flags", "sh");
            // }
            // else
            // {
            //     UserKeyValue.Add("b_flags", "s");
            // }
        }

        public void UpdateUserKeyValues(Dictionary<string, string> data)
        {
            // TODO check if all key is send through the request or
            // TODO only updated key send through the request
            foreach (var key in data.Keys)
            {
                if (UserKeyValue.ContainsKey(key))
                {
                    //we update the key value
                    UserKeyValue[key] = data[key];
                }
                else
                {
                    UserKeyValue.Add(key, data[key]);
                }
            }
        }

        public string GetUserValues(List<string> keys)
        {
            string values = "";
            foreach (var key in keys)
            {
                if (UserKeyValue.ContainsKey(key))
                {
                    values += @"\" + UserKeyValue[key];
                }
            }
            return values;
        }
    }
}