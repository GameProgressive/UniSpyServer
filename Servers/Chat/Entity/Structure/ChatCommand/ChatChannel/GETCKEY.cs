using System;
using System.Collections.Generic;
using GameSpyLib.Extensions;

namespace Chat.Entity.Structure.ChatCommand
{
    public enum GetKeyType
    {
        GetChannelUsersKeyValue,
        GetChannelUserKeyValue
    }
    public class GETCKEY : ChatChannelCommandBase
    {
        public string NickName { get; protected set; }
        public string Cookie { get; protected set; }
        public List<string> Keys { get; protected set; }

        public GetKeyType RequestType { get; protected set; }

        public GETCKEY()
        {
            Keys = new List<string>();
        }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }

            if (_cmdParams.Count != 4)
            {
                return false;
            }

            if (_longParam == null)
            {
                return false;
            }

            NickName = _cmdParams[1];

            if (NickName == "*")
            {
                RequestType = GetKeyType.GetChannelUsersKeyValue;
            }
            else
            {
                RequestType = GetKeyType.GetChannelUserKeyValue;
            }

            Cookie = _cmdParams[2];

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);

            return true;
        }
    }
}
