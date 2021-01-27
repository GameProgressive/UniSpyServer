using Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request.Channel
{
    internal sealed class GETCHANKEYRequest : ChatChannelRequestBase
    {
        public GETCHANKEYRequest(string rawRequest) : base(rawRequest)
        {
        }
        public string Cookie { get; private set; }
        public List<string> Keys { get; private set; }

        public override void Parse()
        {
            base.Parse();
            if(ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_cmdParams.Count != 3)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_longParam == null || _longParam.Last() != '\0')
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);
        }
    }
}
