using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    
    public sealed class SetChannelKeyRequest : ChannelRequestBase
    {
        public Dictionary<string, string> KeyValue { get; private set; }

        public SetChannelKeyRequest(string rawRequest) : base(rawRequest){ }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                throw new Exception.ChatException("Channel keys and values are missing.");
            }
            _longParam = _longParam.Substring(1);

            KeyValue = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
