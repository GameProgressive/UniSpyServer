using Chat.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETCHANKEYRequest : ChatChannelRequestBase
    {
        public Dictionary<string, string> KeyValue { get; protected set; }

        public SETCHANKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();
            if(!ErrorCode)
            {
               ErrorCode = false;
                return;
            }

            if (_longParam == null)
            {
                ErrorCode = false;
                return;
            }
            _longParam = _longParam.Substring(1);

            KeyValue = StringExtensions.ConvertKVStrToDic(_longParam);

            ErrorCode = true;
        }
    }
}
