using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using System.Collections.Generic;
using System.Linq;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    [RequestContract("GETCHANKEY")]
    public sealed class GetChannelKeyRequest : ChannelRequestBase
    {
        public GetChannelKeyRequest(string rawRequest) : base(rawRequest)
        {
        }
        public string Cookie { get; private set; }
        public List<string> Keys { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 3)
            {
                throw new Exception.Exception("The cmdParams number is invalid.");
            }

            if (_longParam == null || _longParam.Last() != '\0')
            {
                throw new Exception.Exception("The longParam number is invalid.");
            }

            Cookie = _cmdParams[1];

            Keys = _longParam.TrimStart('\\').TrimEnd('\0').Split("\\").ToList();
        }
    }
}
