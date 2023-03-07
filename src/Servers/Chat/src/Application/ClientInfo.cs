using System.Collections.Concurrent;
using System.Collections.Generic;
using Newtonsoft.Json;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
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
        public IDictionary<string, Channel> JoinedChannels { get; private set; }
        // secure connection
        public string GameName { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ServerIP { get; set; }
        public int NameSpaceID { get; set; }
        public string UniqueNickName { get; set; }
        public string GameSecretKey { get; set; }
        public bool IsLoggedIn { get; set; }
        public bool IsUsingEncryption { get; set; }
        public bool IsQuietMode { get; set; }
        public string IRCPrefix => $"{NickName}!{UserName}@{ChatConstants.ServerDomain}";
        public bool IsRemoteClient { get; set; }
        public Dictionary<string, string> GlobalKeyValue { get; private set; }
        public ClientInfo()
        {
            JoinedChannels = new ConcurrentDictionary<string, Channel>();
            GlobalKeyValue = new Dictionary<string, string>();
            NameSpaceID = 0;
            IsUsingEncryption = false;
            IsQuietMode = false;
            IsLoggedIn = false;
        }
        public void UpdateUserKeyValues(Dictionary<string, string> data)
        {
            // TODO check if all key is send through the request or
            // TODO only updated key send through the request
            foreach (var key in data.Keys)
            {
                if (GlobalKeyValue.ContainsKey(key))
                {
                    //we update the key value
                    GlobalKeyValue[key] = data[key];
                }
                else
                {
                    GlobalKeyValue.Add(key, data[key]);
                }
            }
        }

        public string GetUserValues(List<string> keys)
        {
            string values = "";
            foreach (var key in keys)
            {
                if (GlobalKeyValue.ContainsKey(key))
                {
                    values += @"\" + GlobalKeyValue[key];
                }
            }
            return values;
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