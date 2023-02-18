using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.MiscMethod;

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
        public IPEndPoint RemoteIPEndPoint
        {
            get
            {
                if (IsRemoteUser)
                {
                    return _remoteIPEndPoint;
                }
                else
                {
                    return ClientRef.Connection.RemoteIPEndPoint;
                }
            }
        }
        [JsonProperty]
        [JsonConverter(typeof(IPEndPointConverter))]
        private IPEndPoint _remoteIPEndPoint;
        [JsonIgnore]
        public Client ClientRef { get; private set; }
        [JsonIgnore]
        public ClientInfo Info
        {
            get
            {
                if (IsRemoteUser)
                {
                    return _info;
                }
                else
                {
                    return ClientRef.Info;
                }
            }
        }
        [JsonProperty]
        private ClientInfo _info;
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
        public ChannelUser(Client client)
        {
            ClientRef = client;
            _remoteIPEndPoint = client?.Connection.RemoteIPEndPoint;
            // RemoteIPEndPoint = client.Connection.RemoteIPEndPoint;
            _info = client?.Info;
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
