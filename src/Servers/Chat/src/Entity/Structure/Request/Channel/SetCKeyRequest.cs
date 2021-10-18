using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request
{
    [RequestContract("SETCKEY")]
    public sealed class SetCKeyRequest : ChannelRequestBase
    {
        public string NickName { get; private set; }

        public Dictionary<string, string> KeyValues { get; private set; }

        public SetCKeyRequest(string rawRequest) : base(rawRequest)
        {
            KeyValues = new Dictionary<string, string>();
        }

        public override void Parse()
        {
            base.Parse();
            if (_longParam == null)
            {
                throw new Exception.Exception("The key value is missing from SETCKEY request.");
            }

            NickName = _cmdParams[1];

            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
