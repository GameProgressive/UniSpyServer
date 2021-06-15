using Chat.Abstraction.BaseClass;
using Chat.Entity.Exception;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request
{
    internal sealed class SETCHANKEYRequest : ChatChannelRequestBase
    {
        public Dictionary<string, string> KeyValue { get; protected set; }

        public SETCHANKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                throw new ChatException("Channel keys and values are missing.");
            }
            _longParam = _longParam.Substring(1);

            KeyValue = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
