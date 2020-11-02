using Chat.Entity.Structure.User;
using Chat.Server;
using System.Collections.Generic;

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

        public void SetDefaultProperties(bool isCreator = false)
        {
            IsVoiceable = true;
            IsChannelCreator = false;
            IsChannelOperator = false;
            UserKeyValue.Add("username", UserInfo.UserName);

            if (isCreator)
            {
                UserKeyValue.Add("b_flags", "sh");
            }
            else
            {
                UserKeyValue.Add("b_flags", "s");
            }
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

        public string BuildReply(string command, string cmdParams)
        {
            return Session.UserInfo.BuildReply(command, cmdParams);
        }

        public string BuildReply(string command, string cmdParams, string tailing)
        {
            return Session.UserInfo.BuildReply(command, cmdParams, tailing);
        }

        public string GetBFlagsString()
        {
            return @"\" + UserInfo.UserName + @"\" + UserKeyValue["b_flags"];
        }

        public string GetUserValuesString(List<string> keys)
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

        public string GetUserModes()
        {
            string buffer = "";

            if (IsChannelOperator)
            {
                buffer += "@";
            }

            if (IsVoiceable)
            {
                buffer += "+";
            }

            return buffer;
        }
    }
}
