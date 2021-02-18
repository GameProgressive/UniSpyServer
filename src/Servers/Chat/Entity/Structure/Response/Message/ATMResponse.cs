﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.Message;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Message
{
    internal sealed class ATMResponse : ChatResponseBase
    {
        private new ATMResult _result => (ATMResult)base._result;
        private new ATMRequest _request => (ATMRequest)base._request;
        public ATMResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            string cmdParams = $"{_result.TargetName} {_request.Message}";
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.UserIRCPrefix, ChatReplyName.ATM, cmdParams);
        }
    }
}