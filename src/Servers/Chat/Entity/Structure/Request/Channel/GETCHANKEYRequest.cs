using Chat.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatCommand.Channel
{
    public class GETCHANKEYRequest : ChatChannelRequestBase
    {
        public GETCHANKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Cookie { get; protected set; }
        public List<string> Keys { get; protected set; }


        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            if (_cmdParams.Count != 3)
            {
                ErrorCode = false;
                return;
            }

            if (_longParam == null || _longParam.Last() != '\0')
            {
                ErrorCode = false;
                return;
            }

            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);

            ErrorCode = true;
        }
    }
}
