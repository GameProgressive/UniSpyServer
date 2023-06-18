using System.Collections.Concurrent;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        /// <summary>
        /// indicates which channel this user is in
        /// (We do not send this information to our public channel)
        /// </summary>
        [JsonIgnore]
        public ConcurrentDictionary<string, Channel> JoinedChannels { get; private set; } = new ConcurrentDictionary<string, Channel>();
        // secure connection
        public string GameName { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ServerIP { get; set; }
        public int NameSpaceID { get; set; } = 0;
        public string UniqueNickName { get; set; }
        public string GameSecretKey { get; set; }
        public bool IsLoggedIn { get; set; } = false;
        public bool IsUsingEncryption { get; set; } = false;
        public bool IsQuietMode { get; set; } = false;
        [JsonIgnore]
        public string IRCPrefix => $"{NickName}!{UserName}@{Chat.Abstraction.BaseClass.ResponseBase.ServerDomain}";
        public bool IsRemoteClient { get; set; }
        public string PreviousJoinedChannel { get; set; } = "";
        /// <summary>
        /// Global user key values
        /// </summary>
        /// <value></value>
        public KeyValueManager KeyValues { get; private set; } = new KeyValueManager();

        public ClientInfo()
        {
        }

        public bool IsJoinedChannel(string channelName) => JoinedChannels.ContainsKey(channelName);

        public Channel GetJoinedChannel(string channelName)
        {
            if (JoinedChannels.ContainsKey(channelName))
            {
                return JoinedChannels[channelName];
            }
            else
            {
                return null;
            }
        }
        public ClientInfo DeepCopy()
        {
            var infoCopy = (ClientInfo)this.MemberwiseClone();
            return infoCopy;
        }
    }
}