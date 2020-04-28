using System.Collections.Generic;
using System.Linq;
using Chat.Entity.Structure.ChatChannel;
using Chat.Handler.SystemHandler.ChannelManage;

namespace Chat.Entity.Structure.ChatUser
{
    public class ChatUserInfo
    {
        //indicates which channel this user is in
        public List<ChatChannelBase> JoinedChannels { get; set; }
        public bool IsQuietMode { get; protected set; }
        public string GameName { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ServerIP { get; set; }
        public int NameSpaceID { get; set; }
        public string UniqueNickName { get; set; }
        public string GameSecretKey { get; set; }
        // secure connection

        public GSPeerChatCTX ClientCTX { get; set; }
        public GSPeerChatCTX ServerCTX { get; set; }

        public bool UseEncryption;

        public ChatUserInfo()
        {
            NameSpaceID = 0;
            UseEncryption = false;
            IsQuietMode = false;
            ClientCTX = new GSPeerChatCTX();
            ServerCTX = new GSPeerChatCTX();
            JoinedChannels = new List<ChatChannelBase>();
        }
        public void SetQuietModeFlag(bool flag)
        {
            IsQuietMode = flag;
        }

        public bool IsJoinedChannel(string channelName)
        {
            return GetJoinedChannel(channelName, out _);
        }

        public bool GetJoinedChannel(string channelName,out ChatChannelBase channel)
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
    }
}