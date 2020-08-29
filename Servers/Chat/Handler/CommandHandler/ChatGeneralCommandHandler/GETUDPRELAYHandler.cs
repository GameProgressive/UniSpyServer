using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatGeneralCommandHandler
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    public class GETUDPRELAYHandler : ChatCommandHandlerBase
    {
        new GETUDPRELAYRequest _request;

        public GETUDPRELAYHandler(ISession session, ChatRequestBase cmd) : base(session, cmd)
        {
            _request = (GETUDPRELAYRequest)cmd;
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
        protected override void DataOperation()
        {
            base.DataOperation();
        }

        protected override void ConstructResponse()
        {
            base.ConstructResponse();
        }

    }
}
