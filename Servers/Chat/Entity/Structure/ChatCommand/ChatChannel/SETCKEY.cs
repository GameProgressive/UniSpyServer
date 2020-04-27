using GameSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETCKEY : ChatChannelCommandBase
    {
        public string UserName { get; protected set; }

        public Dictionary<string, string> KeyValues { get; protected set; }

        public SETCKEY()
        {
            KeyValues = new Dictionary<string, string>();
        }
        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            { return false; }
            if (_longParam == null)
            { return false; }

            UserName = _cmdParams[1];

            _longParam = _longParam.Substring(1);

            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);

            return true;
        }
    }
}
