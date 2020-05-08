using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatUser;
using Chat.Server;
using System.Collections.Generic;
using System.Net;

namespace Chat.Entity.Structure.ChatChannel
{
    public class ChatChannelUser
    {
        public bool IsVoiceable { get; protected set; }
        public bool IsChannelCreator { get; protected set; }
        public bool IsChannelOperator { get; protected set; }
        public ChatSession Session { get; protected set; }
        public ChatUserInfo UserInfo { get; protected set; }

        public Dictionary<string, string> UserKeyValue { get; protected set; }

        public ChatChannelUser(ChatSession session)
        {
            Session = session;
            UserInfo = session.UserInfo;
            UserKeyValue = new Dictionary<string, string>();
        }

        public ChatChannelUser()
        {
        }

        public void SetDefaultProperties()
        {
            IsVoiceable = true;
            IsChannelCreator = false;
            IsChannelOperator = false;
            UserKeyValue.Add("username", UserInfo.UserName);
            UserKeyValue.Add("b_flags", "s");
        }

        public void SetCreatorProperties()
        {
            IsVoiceable = true;
            IsChannelCreator = true;
            IsChannelOperator = true;
            UserKeyValue.Add("username", UserInfo.UserName);
            UserKeyValue.Add("b_flags", "sh");
        }

        public ChatChannelUser SetVoicePermission(bool flag = true)
        {
            IsVoiceable = flag;
            return this;
        }
        public ChatChannelUser SetChannelCreator(bool flag = false)
        {
            IsChannelCreator = flag;
            return this;
        }
        public ChatChannelUser SetChannelOperator(bool flag = false)
        {
            IsChannelOperator = flag;
            return this;
        }

        public void UpdateUserKeyValue(Dictionary<string, string> data)
        {
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

        public string BuildChannelMessage(string cmdParam, string message)
        {
            string address = ((IPEndPoint)Session.Socket.RemoteEndPoint).Address.ToString();
            return ChatCommandBase.BuildMessageRPL(
                    $"{UserInfo.NickName}!{UserInfo.UserName}@{address}", cmdParam, message);
        }

        public string BuildChannelMessage(string cmdParam)
        {
            return BuildChannelMessage(cmdParam,"");
        }
    }
}
