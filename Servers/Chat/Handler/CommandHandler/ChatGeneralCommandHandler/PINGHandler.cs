using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    public class PINGHandler : ChatCommandHandlerBase
    {
        new PINGRequest _request;
        public PINGHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
            _request = new PINGRequest(request.RawRequest);
        }
        protected override void CheckRequest()
        {
            base.CheckRequest();
            if (!_request.Parse())
            {
                _errorCode = ChatError.Parse;
                return;
            }
        }
        protected override void ConstructResponse()
        {
            base.ConstructResponse();
            _sendingBuffer = ChatReply.BuildPingReply(_session.UserInfo);
        }
    }
}
