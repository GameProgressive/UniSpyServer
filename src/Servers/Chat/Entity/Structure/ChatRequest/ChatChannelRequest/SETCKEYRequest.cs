using GameSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{

    public class SETCKEYRequest : ChatChannelRequestBase
    {
        public string NickName { get; protected set; }

        public Dictionary<string, string> KeyValues { get; protected set; }

        public SETCKEYRequest(string rawRequest) : base(rawRequest)
        {
            KeyValues = new Dictionary<string, string>();
        }

        public override bool Parse()
        {
            if (!base.Parse())
            {
                return false;
            }
            if (_longParam == null)
            { return false; }

            NickName = _cmdParams[1];

            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);

            return true;
        }
    }
}
