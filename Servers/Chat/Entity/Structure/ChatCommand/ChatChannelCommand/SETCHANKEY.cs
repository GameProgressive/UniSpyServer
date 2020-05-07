using GameSpyLib.Extensions;
using System.Collections.Generic;

namespace Chat.Entity.Structure.ChatCommand
{
    public class SETCHANKEY : ChatChannelCommandBase
    {
        public Dictionary<string, string> KeyValue { get; protected set; }

        public SETCHANKEY()
        {
            KeyValue = new Dictionary<string, string>();
        }

        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }
            if (_longParam == null)
                return false;

            _longParam = _longParam.Substring(1);

            KeyValue = StringExtensions.ConvertKVStrToDic(_longParam);

            return true;
        }
    }
}
