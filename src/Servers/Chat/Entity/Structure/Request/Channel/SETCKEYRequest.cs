using Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.ChatCommand
{

    public class SETCKEYRequest : ChatChannelRequestBase
    {
        public string NickName { get; protected set; }

        public Dictionary<string, string> KeyValues { get; protected set; }

        public SETCKEYRequest(string rawRequest) : base(rawRequest)
        {
            KeyValues = new Dictionary<string, string>();
        }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
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

            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);

            ErrorCode = true;
        }
    }
}
