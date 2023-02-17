using System.Collections.Concurrent;
using System.Collections.Generic;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Application
{
    public sealed class ClientInfo : ClientInfoBase
    {
        //indicates which channel this user is in
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

        public ClientInfo( )
        {
            JoinedChannels = new ConcurrentDictionary<string, Channel>();
            NameSpaceID = 0;
            IsUsingEncryption = false;
            IsQuietMode = false;
            IsLoggedIn = false;
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
    }
}