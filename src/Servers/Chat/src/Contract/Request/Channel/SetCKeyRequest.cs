using UniSpy.Server.Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpy.Server.Core.Extension;
using System.Linq;

namespace UniSpy.Server.Chat.Contract.Request.Channel
{

    public sealed class SetCKeyRequest : ChannelRequestBase
    {
        public string Channel { get; private set; }
        public string NickName { get; private set; }
        public string Cookie { get; private set; }
        public bool IsBroadCast { get; private set; }
        public string KeyValueString { get; private set; }
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
                throw new Chat.Exception("The cmdParams from SETCKEY request are missing.");
            }

            if (_longParam is null)
            {
                throw new Chat.Exception("The longParam from SETCKEY request is missing.");
            }

            Channel = _cmdParams[0];
            NickName = _cmdParams[1];
            _longParam = _longParam.Substring(1);
            KeyValues = StringExtensions.ConvertKVStringToDictionary(_longParam);
            KeyValueString = _longParam;
            // todo check whether there exists other b_* key value
            if (KeyValues.Keys.Where(x => x.Contains("b_")).Count() > 0)
            {
                Cookie = "BCAST";
                IsBroadCast = true;
            }
        }
    }
}
