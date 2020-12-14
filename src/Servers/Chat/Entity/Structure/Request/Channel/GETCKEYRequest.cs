using Chat.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public enum GetKeyType
    {
        GetChannelAllUserKeyValue,
        GetChannelSpecificUserKeyValue
    }
    public class GETCKEYRequest : ChatChannelRequestBase
    {
        public string NickName { get; protected set; }
        public string Cookie { get; protected set; }
        public List<string> Keys { get; protected set; }

        public GetKeyType RequestType { get; protected set; }

        public GETCKEYRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            if (_cmdParams.Count != 4)
            {
                ErrorCode = false;
                return;
            }

            if (_longParam == null)
            {
                ErrorCode = false;
                return;
            }

            NickName = _cmdParams[1];

            if (NickName == "*")
            {
                RequestType = GetKeyType.GetChannelAllUserKeyValue;
            }
            else
            {
                RequestType = GetKeyType.GetChannelSpecificUserKeyValue;
            }

            Cookie = _cmdParams[2];

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);

            ErrorCode = true;
        }
    }
}
