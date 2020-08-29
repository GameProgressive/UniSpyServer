using GameSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETKEY : ChatRequestBase
    {
        public Dictionary<string, string> KeyValues { get; protected set; }

        public SETKEY(string rawRequest) : base(rawRequest)
        {
            KeyValues = new Dictionary<string, string>();
        }

        public override bool Parse()
        {
            if (!Parse())
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
