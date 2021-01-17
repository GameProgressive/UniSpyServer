using Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.ChatCommand
{
    internal enum GetKeyType
    {
        GetChannelAllUserKeyValue,
        GetChannelSpecificUserKeyValue
    }
    internal sealed class GETCKEYRequest : ChatChannelRequestBase
    {
        public string NickName { get; private set; }
        public string Cookie { get; private set; }
        public List<string> Keys { get; private set; }

        public GetKeyType RequestType { get; private set; }

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
