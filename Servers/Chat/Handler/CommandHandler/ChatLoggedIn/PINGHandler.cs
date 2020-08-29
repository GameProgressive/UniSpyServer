using System;
using Chat.Entity.Structure.ChatCommand;
using Chat.Entity.Structure.ChatResponse;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class PINGHandler : ChatCommandHandlerBase
    {
        public PINGHandler(ISession session, ChatRequestBase request) : base(session, request)
        {
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
            _sendingBuffer = ChatReply.BuildPingReply(_session.UserInfo);
        }
    }
}
