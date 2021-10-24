using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Chat.Entity.Structure.Request.Channel
{
    public enum GetKeyType
    {
        GetChannelAllUserKeyValue,
        GetChannelSpecificUserKeyValue
    }

    [RequestContract("GETCKEY")]
    public sealed class GetCKeyRequest : ChannelRequestBase
    {
        public string NickName { get; private set; }
        public string Cookie { get; private set; }
        public List<string> Keys { get; private set; }
        public GetKeyType RequestType { get; private set; }
        public GetCKeyRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 4)
            {
                throw new Exception.Exception("number of IRC parameters are incorrect.");
            }

            if (_longParam == null)
            {
                throw new Exception.Exception("IRC long parameter is incorrect.");
            }

            NickName = _cmdParams[1];

            if (NickName == "*")
            {
                RequestType = GetKeyType.GetChannelAllUserKeyValue;
            }
            else
            {
                RequestType = GetKeyType.GetChannelSpecificUserKeyValue;
            }

            Cookie = _cmdParams[2];

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);
        }
    }
}
