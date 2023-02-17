using UniSpy.Server.Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpy.Server.Core.Extensions;

namespace UniSpy.Server.Chat.Contract.Request.Channel
{
    
    public sealed class SetCKeyRequest : ChannelRequestBase
    {
        public string Channel { get; private set; }

        public string NickName { get; private set; }

        public Dictionary<string, string> KeyValues { get; private set; }

        public SetCKeyRequest(string rawRequest) : base(rawRequest)
        {
            KeyValues = new Dictionary<string, string>();
        }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams is null)
            {
                throw new Exception.ChatException("The cmdParams from SETCKEY request are missing.");
            }

            if (_longParam is null)
            {
                throw new Exception.ChatException("The longParam from SETCKEY request is missing.");
            }

            Channel = _cmdParams[0];
            NickName = _cmdParams[1];

            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStringToDictionary(_longParam);
        }
    }
}
