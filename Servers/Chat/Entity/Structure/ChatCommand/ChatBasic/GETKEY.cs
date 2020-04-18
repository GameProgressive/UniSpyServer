using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Entity.Structure.ChatCommand.ChatBasic
{
    public class GETKEY : ChatCommandBase
    {
        public string Target { get; protected set; }
        public string Cookie { get; protected set; }
        public List<string> Keys { get; protected set; }
        public GETKEY(string request) : base(request)
        {
        }
        public override bool Parse()
        {
            if (!base.Parse())
            { return false; }

            if (_cmdParams.Count < 3)
            {
                return false;
            }
            Target = _cmdParams[0];
            Cookie = _cmdParams[1];
            if (_longParam == null)
            {
                return false;
            }

            if (_longParam.Last() != '\0')
            {
                return false;
            }

            List<string> keyList =
                _longParam.TrimStart('\\').Split('\\',StringSplitOptions.RemoveEmptyEntries).ToList();

            foreach (var key in keyList)
            {
                key.Replace('/', '\\');
            }

            return true;
        }
    }
}
