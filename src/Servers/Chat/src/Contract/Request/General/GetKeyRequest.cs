using UniSpy.Server.Chat.Abstraction.BaseClass;
using System.Collections.Generic;
using UniSpy.Server.Core.Extension;

namespace UniSpy.Server.Chat.Contract.Request.General
{
    /// <summary>
    /// Retrieves a list of global key/value for a single user, or all users.
    /// </summary>

    public sealed class GetKeyRequest : RequestBase
    {
        public bool IsGetAllUser { get; private set; }
        public string NickName { get; private set; }
        public string Cookie { get; private set; }
        public string UnkownCmdParam { get; private set; }
        public List<string> Keys { get; private set; }
        public GetKeyRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count < 2)
            {
                throw new Chat.Exception("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            if (_longParam is null)
            {
                throw new Chat.Exception("The number of IRC long params in GETKEY request is incorrect.");
            }

            NickName = _cmdParams[0];
            Cookie = _cmdParams[1];
            UnkownCmdParam = _cmdParams[2];

            _longParam = _longParam.Substring(0, _longParam.Length);
            if (NickName == "*")
            {
                IsGetAllUser = true;
            }
            Keys = StringExtensions.ConvertKeyStrToList(_longParam);
        }
    }
}
