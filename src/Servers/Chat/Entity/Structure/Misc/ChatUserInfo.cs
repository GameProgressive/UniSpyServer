using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Network;
using System.Collections.Concurrent;
using System.Linq;
namespace Chat.Entity.Structure.Misc
{
    internal sealed class ChatUserInfo
    {
        //indicates which channel this user is in
        public ConcurrentBag<ChatChannel> JoinedChannels { get; private set; }
        public ChatSession Session { get; private set; }
        // secure connection
        public GSPeerChatCTX ClientCTX { get; set; }
        public GSPeerChatCTX ServerCTX { get; private set; }
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
        public string PublicIPAddress => Session.RemoteIPEndPoint.Address.ToString();
        public string IRCPrefix => $"{NickName}!{UserName}@{ChatConstants.ServerDomain}";

        public ChatUserInfo(ChatSession session)
        {
            Session = session;
            ClientCTX = new GSPeerChatCTX();
            ServerCTX = new GSPeerChatCTX();
            JoinedChannels = new ConcurrentBag<ChatChannel>();
            NameSpaceID = 0;
            IsUsingEncryption = false;
            IsQuietMode = false;
            IsLoggedIn = false;
        }

        public bool IsJoinedChannel(string channelName)
        {
            if (JoinedChannels.Where(c => c.Property.ChannelName == channelName).Count() == 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public ChatChannel GetJoinedChannelByName(string channelName)
        {
            var result = JoinedChannels.Where(c => c.Property.ChannelName == channelName);
            if (result.Count() == 1)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }
    }
}