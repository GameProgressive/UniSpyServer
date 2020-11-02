using Chat.Abstraction.BaseClass;
using System.Collections.Concurrent;
using System.Linq;
using System.Net;

namespace Chat.Entity.Structure.User
{
    public class ChatUserInfo
    {
        //indicates which channel this user is in
        public ConcurrentBag<ChatChannelBase> JoinedChannels { get; protected set; }
        public bool IsQuietMode { get; protected set; }
        public string PublicIPAddress { get; protected set; }


        public ChatUserInfo()
        {
            ClientCTX = new GSPeerChatCTX();
            ServerCTX = new GSPeerChatCTX();
            JoinedChannels = new ConcurrentBag<ChatChannelBase>();
        }

        public ChatUserInfo SetQuietModeFlag(bool flag)
        {
            IsQuietMode = flag;
            return this;
        }

        public string GameName { get; protected set; }
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
        public string ServerIP { get; protected set; }
        public ChatUserInfo SetServerIP(string ip)
        {
            ServerIP = ip;
            return this;
        }
        public int NameSpaceID { get; protected set; }
        public ChatUserInfo SetNameSpaceID(int namespaceid)
        {
            NameSpaceID = namespaceid;
            return this;
        }
        public string UniqueNickName { get; protected set; }
        public ChatUserInfo SetUniqueNickName(string uniquenick)
        {
            UniqueNickName = uniquenick;
            return this;
        }
        public string GameSecretKey { get; protected set; }
        public ChatUserInfo SetGameSecretKey(string key)
        {
            GameSecretKey = key;
            return this;
        }
        public bool IsLoggedIn { get; protected set; }
        public ChatUserInfo SetLoginFlag(bool flag)
        {
            IsLoggedIn = flag;
            return this;
        }

        // secure connection
        public GSPeerChatCTX ClientCTX { get; protected set; }
        public ChatUserInfo SetClientCTX(GSPeerChatCTX ctx)
        {
            ClientCTX = ctx;
            return this;
        }
        public GSPeerChatCTX ServerCTX { get; protected set; }
        public ChatUserInfo SetServerCTX(GSPeerChatCTX ctx)
        {
            ServerCTX = ctx;
            return this;
        }

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
        public bool GetJoinedChannelByName(string channelName, out ChatChannelBase channel)
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
            return ChatResponseBase.BuildResponse(this, command, cmdParams, tailing);
        }
    }
}