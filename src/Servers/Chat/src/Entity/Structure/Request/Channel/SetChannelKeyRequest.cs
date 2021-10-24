using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Chat.Entity.Structure.Request.Channel
{
    [RequestContract("SETCHANKEY")]
    public sealed class SetChannelKeyRequest : ChannelRequestBase
    {
        public Dictionary<string, string> KeyValue { get; private set; }

        public SetChannelKeyRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (_longParam == null)
            {
                throw new Exception.Exception("Channel keys and values are missing.");
            }
            _longParam = _longParam.Substring(1);

            KeyValue = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
