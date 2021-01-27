using Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request
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

            NickName = _cmdParams[1];

            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);
        }
    }
}
