using GameSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class SETCHANKEY : ChatChannelCommandBase
    {
        public Dictionary<string, string> KeyValues { get; protected set; }
        public SETCHANKEY(string request) : base(request)
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
                return false;

            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);

            return true;
        }
    }
}
