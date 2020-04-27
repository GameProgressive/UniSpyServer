using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Extensions;

namespace Chat.Entity.Structure.ChatCommand
{
    public class GETCHANKEY : ChatChannelCommandBase
    {
        public GETCHANKEY()
        {
            Keys = new List<string>();
        }

        public string Cookie { get; protected set; }
        public List<string> Keys { get; protected set; }


        public override bool Parse(string request)
        {
            if (!base.Parse(request))
            {
                return false;
            }

            if (_cmdParams.Count != 3)
            {
                return false;
            }

            if (_longParam == null || _longParam.Last() != '\0')
            {
                return false;
            }

            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0,_longParam.Length-2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);

            return true;
        }
    }
}
