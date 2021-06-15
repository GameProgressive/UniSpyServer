using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request
{

    internal sealed class SETCKEYRequest : ChatChannelRequestBase
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
            if (_longParam == null)
            {
                throw new ChatException("The key value is missing from SETCKEY request.");
            }

            NickName = _cmdParams[1];

            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
