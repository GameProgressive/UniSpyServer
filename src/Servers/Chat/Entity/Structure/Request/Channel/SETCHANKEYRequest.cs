using Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request
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
            if(ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_longParam == null)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }
            _longParam = _longParam.Substring(1);

            KeyValue = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
