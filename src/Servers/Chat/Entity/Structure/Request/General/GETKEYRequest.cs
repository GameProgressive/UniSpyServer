using Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpyLib.Extensions;

namespace Chat.Entity.Structure.Request
{
    /// <summary>
    /// Retrieves a list of global key/value for a single user, or all users.
    /// </summary>
    public class GETKEYRequest : ChatRequestBase
    {
        public string NickName { get; protected set; }
        public string Cookie { get; protected set; }
        public List<string> Keys { get; protected set; }
        public GETKEYRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();
            if(ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }


            if (_cmdParams.Count < 3)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_longParam == null)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            NickName = _cmdParams[0];
            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);
        }
    }
}
