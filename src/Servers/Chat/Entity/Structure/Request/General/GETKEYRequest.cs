using Chat.Abstraction.BaseClass;
using UniSpyLib.Extensions;
using System.Collections.Generic;

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

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }


            if (_cmdParams.Count < 3)
            {
                ErrorCode = false;
                return;
            }

            if (_longParam == null)
            {
                ErrorCode = false;
                return;
            }

            NickName = _cmdParams[0];
            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);

            ErrorCode = true;
        }
    }
}
