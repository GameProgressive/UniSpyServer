using Chat.Entity.Structure.Misc.ChannelInfo;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;
namespace Chat.Entity.Structure.Misc
{
    public class ChatUserInfo
    {
        //indicates which channel this user is in
        public ConcurrentBag<ChatChannel> JoinedChannels { get; protected set; }
        public bool IsQuietMode { get; set; }
        public string PublicIPAddress { get; protected set; }


        public ChatUserInfo()
        {
            ClientCTX = new GSPeerChatCTX();
            ServerCTX = new GSPeerChatCTX();
            JoinedChannels = new ConcurrentBag<ChatChannel>();
        }


        public string GameName { get; set; }
        public ChatUserInfo SetGameName(string gameName)
        {
            GameName = gameName;
            return this;
        }
        public string NickName { get; protected set; }
        public ChatUserInfo SetNickName(string nickName)
        {
            NickName = nickName;
            return this;
        }
        public string UserName { get; protected set; }
        public ChatUserInfo SetUserName(string userName)
        {
            UserName = userName;
            return this;
        }
        public string Name { get; protected set; }
        public ChatUserInfo SetName(string name)
        {
            Name = name;
            return this;
        }
        public string ServerIP { get; set; }
        public int NameSpaceID { get; set; }

        public string UniqueNickName { get; set; }
        public string GameSecretKey { get; set; }
        public bool IsLoggedIn { get; set; }

        // secure connection
        public GSPeerChatCTX ClientCTX { get; set; }

        public GSPeerChatCTX ServerCTX { get; protected set; }


        public bool UseEncryption { get; protected set; }
        public ChatUserInfo SetUseEncryptionFlag(bool flag)
        {
            UseEncryption = flag;
            return this;
        }


        public ChatUserInfo SetDefaultUserInfo(EndPoint endPoint)
        {
            NameSpaceID = 0;
            UseEncryption = false;
            IsQuietMode = false;
            IsLoggedIn = false;
            PublicIPAddress = ((IPEndPoint)endPoint).Address.ToString();
            return this;
        }


        public bool IsJoinedChannel(string channelName)
        {
            return GetJoinedChannelByName(channelName, out _);
        }
        public bool GetJoinedChannelByName(string channelName, out ChatChannel channel)
        {
            var result = JoinedChannels.Where(c => c.Property.ChannelName == channelName);
            if (result.Count() == 1)
            {
                channel = result.First();
                return true;
            }
            else
            {
                channel = null;
                return false;
            }
        }

        public string BuildReply(string command)
        {
            return BuildReply(command, "");
        }
        public string BuildReply(string command, string cmdParams)
        {
            return BuildReply(command, cmdParams, "");
        }

        public string BuildReply(string command, string cmdParams, string tailing)
        {
            return ChatReplyBuilder.Build(this, command, cmdParams, tailing);
        }
    }
}