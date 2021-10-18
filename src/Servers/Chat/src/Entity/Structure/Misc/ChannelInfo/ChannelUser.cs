using System.Collections.Generic;

namespace Chat.Entity.Structure.Misc.ChannelInfo
{
    public sealed class ChannelUser
    {
        public bool IsVoiceable { get; set; }
        public bool IsChannelCreator { get; set; }
        public bool IsChannelOperator { get; set; }
        public UserInfo UserInfo { get; private set; }
        public Dictionary<string, string> UserKeyValue { get; private set; }

        public string BFlags => @"\" + UserInfo.UserName + @"\" + UserKeyValue["b_flags"];

        public ChannelUser(UserInfo userInfo)
        {
            UserInfo = userInfo;
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

        public void UpdateUserKeyValues(Dictionary<string, string> data)
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
