using System;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    public class GETUDPRELAYHandler : ChatCommandHandlerBase
    {
        new GETUDPRELAY _request;

        public GETUDPRELAYHandler(ISession session, ChatRequestBase cmd) : base(session, cmd)
        {
            _request = (GETUDPRELAY)cmd;
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
