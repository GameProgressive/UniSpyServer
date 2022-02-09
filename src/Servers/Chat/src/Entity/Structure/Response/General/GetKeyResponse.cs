﻿using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class GetKeyResponse : ResponseBase
    {
        private new GetKeyResult _result => (GetKeyResult)base._result;
        private new GetKeyRequest _request => (GetKeyRequest)base._request;
        public GetKeyResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = "";
            foreach (var flag in _result.Flags)
            {
                string cmdParams1 = $"param1 {_result.NickName} {_request.Cookie} {flag}";
                SendingBuffer += IRCReplyBuilder.Build(ResponseName.GetKey, cmdParams1);
            }
            string cmdParams2 = $"param1 param2 {_request.Cookie} param4";
            string tailing = "End of GETKEY.";
            SendingBuffer += IRCReplyBuilder.Build(ResponseName.EndGetKey, cmdParams2, tailing);
        }
    }
}
