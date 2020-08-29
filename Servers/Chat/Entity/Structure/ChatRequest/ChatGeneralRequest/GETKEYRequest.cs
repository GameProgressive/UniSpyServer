using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Extensions;

namespace Chat.Entity.Structure.ChatCommand
{
    /// <summary>
    /// Retrieves a list of global key/value for a single user, or all users.
    /// </summary>
    public class GETKEYRequest : ChatRequestBase
    {

        public GETKEYRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public string NickName { get; protected set; }
        public string Cookie { get; protected set; }

        public List<string> Keys { get; protected set; }

        public override bool Parse()
        {
            if (!Parse())
            { return false; }

            if (_cmdParams.Count < 3)
            {
                return false;
            }

            if (_longParam == null)
            {
                return false;
            }

            NickName = _cmdParams[0];
            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);

            return true;
        }
    }
}
