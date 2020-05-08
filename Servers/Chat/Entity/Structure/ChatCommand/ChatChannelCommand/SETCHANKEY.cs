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

        public override bool Parse(string recv)
        {
            if (!base.Parse(recv))
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
