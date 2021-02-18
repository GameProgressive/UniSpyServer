﻿using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Message
{
    internal sealed class UTMResponse : ChatResponseBase
    {
        private new UTMResult _result => (UTMResult)base._result;
        private new UTMRequest _request => (UTMRequest)base._request;
        public UTMResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        
        protected override void BuildNormalResponse()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.UserIRCPrefix, ChatReplyName.UTM, _result.Name, _request.Message);
        }
    }
}