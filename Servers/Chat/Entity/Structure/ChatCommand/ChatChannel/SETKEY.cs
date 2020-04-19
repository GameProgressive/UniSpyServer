using GameSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETKEY : ChatCommandBase
    {
        public Dictionary<string, string> KeyValues { get; protected set; }

        public SETKEY()
        {
            KeyValues = new Dictionary<string, string>();
        }
        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            if (_longParam == null)
                return false;
            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);
            return true;
        }
    }
}
