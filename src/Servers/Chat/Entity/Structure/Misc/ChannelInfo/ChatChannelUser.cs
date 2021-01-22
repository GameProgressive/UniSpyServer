using Chat.Network;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Misc.ChannelInfo
{
    public class ChatChannelUser
    {
        public bool IsVoiceable { get; set; }
        public bool IsChannelCreator { get; set; }
        public bool IsChannelOperator { get; set; }
        public ChatSession Session { get; protected set; }
        public ChatUserInfo UserInfo
        {
            get { return Session.UserInfo; }
        }
        public string BFlags
        {
            get { return @"\" + UserInfo.UserName + @"\" + UserKeyValue["b_flags"]; }
        }

        public Dictionary<string, string> UserKeyValue { get; protected set; }

        public ChatChannelUser(ChatSession session)
        {
            Session = session;
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
