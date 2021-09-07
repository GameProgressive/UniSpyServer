using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Exception;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request.Channel
{
    [RequestContract("GETCHANKEY")]
    internal sealed class GetChannelKeyRequest : ChannelRequestBase
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
                throw new ChatException("cmdParams number is invalid.");
            }

            if (_longParam == null || _longParam.Last() != '\0')
            {
                throw new ChatException("long parameter is incorrect.");
            }

            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);
        }
    }
}
