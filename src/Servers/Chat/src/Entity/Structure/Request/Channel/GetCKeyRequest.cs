using System.Collections.Generic;
using System.Linq;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel
{
    public enum GetKeyReqeustType
    {
        GetChannelAllUserKeyValue,
        GetChannelSpecificUserKeyValue
    }

    [RequestContract("GETCKEY")]
    public sealed class GetCKeyRequest : ChannelRequestBase
    {
        public GetKeyReqeustType RequestType { get; private set; }
        public string NickName { get; private set; }
        public string Cookie { get; private set; }
        public List<string> Keys { get; private set; }
        public GetCKeyRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count != 4)
            {
                throw new Exception.ChatException("The number of IRC parameters are incorrect.");
            }

            if (_longParam == null)
            {
                throw new Exception.ChatException("The IRC long parameter is incorrect.");
            }

            NickName = _cmdParams[1];

            if (NickName == "*")
            {
                RequestType = GetKeyReqeustType.GetChannelAllUserKeyValue;
            }
            else
            {
                RequestType = GetKeyReqeustType.GetChannelSpecificUserKeyValue;
            }
            Cookie = _cmdParams[2];

            if (!_longParam.Contains("\0") && !_longParam.Contains("\\"))
            {
                throw new Exception.ChatException("The key provide is incorrect.");
            }

            Keys = _longParam.TrimStart('\\').TrimEnd('\0').Split("\\").ToList();
        }
    }
}
