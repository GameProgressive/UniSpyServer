using System;
using System.Collections.Generic;
using GameSpyLib.Extensions;

namespace Chat.Entity.Structure.ChatCommand.ChatChannel
{
    public class SETCKEY : ChatChannelCommandBase
    {
        public string UserName { get; protected set; }
        public Dictionary<string, string> KeyValues { get; protected set; }

        public SETCKEY(string request) : base(request)
        {
            KeyValues = new Dictionary<string, string>();
        }
        public override bool Parse()
        {
            if (!base.Parse())
            { return false; }
            if (_longParam == null)
            { return false; }

            KeyValues = StringExtensions.ConvertKVStrToDic(_longParam);

            return true;
        }
    }
}
