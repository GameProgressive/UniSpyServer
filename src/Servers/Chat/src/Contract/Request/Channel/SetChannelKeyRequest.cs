using UniSpy.Server.Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.Chat.Contract.Request.Channel
{

    public sealed class SetChannelKeyRequest : ChannelRequestBase
    {
        public string KeyValueString { get; private set; }
        public Dictionary<string, string> KeyValues { get; private set; }

        public SetChannelKeyRequest(string rawRequest) : base(rawRequest) { }

        public override void Parse()
        {
            base.Parse();

            if (_longParam is null)
            {
                throw new Exception.ChatException("Channel keys and values are missing.");
            }
            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStringToDictionary(_longParam);
            KeyValueString = _longParam;
        }
    }
}
