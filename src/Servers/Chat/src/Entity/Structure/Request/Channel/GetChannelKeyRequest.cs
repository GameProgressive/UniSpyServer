using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Chat.Entity.Structure.Request.Channel
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
                throw new Exception.Exception("cmdParams number is invalid.");
            }

            if (_longParam == null || _longParam.Last() != '\0')
            {
                throw new Exception.Exception("long parameter is incorrect.");
            }

            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);
        }
    }
}
