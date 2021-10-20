﻿using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Exception;
using System.Collections.Generic;
using UniSpyServer.UniSpyLib.Extensions;

namespace UniSpyServer.Chat.Entity.Structure.Request
{
    /// <summary>
    /// Retrieves a list of global key/value for a single user, or all users.
    /// </summary>
    [RequestContract("GETKEY")]
    public sealed class GetKeyRequest : RequestBase
    {
        public string NickName { get; private set; }
        public string Cookie { get; private set; }
        public List<string> Keys { get; private set; }
        public GetKeyRequest(string rawRequest) : base(rawRequest)
        {
            Keys = new List<string>();
        }

        public override void Parse()
        {
            base.Parse();
            if (_cmdParams.Count < 3)
            {
                throw new Exception.Exception("The number of IRC cmd params in GETKEY request is incorrect.");
            }

            if (_longParam == null)
            {
                throw new Exception.Exception("The number of IRC long params in GETKEY request is incorrect.");
            }

            NickName = _cmdParams[0];
            Cookie = _cmdParams[1];

            _longParam = _longParam.Substring(0, _longParam.Length - 2);

            Keys = StringExtensions.ConvertKeyStrToList(_longParam);
        }
    }
}
