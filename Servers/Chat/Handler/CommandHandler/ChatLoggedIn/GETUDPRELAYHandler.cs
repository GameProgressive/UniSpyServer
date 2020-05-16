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
        new GETUDPRELAY _cmd;

        public GETUDPRELAYHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
            _cmd = (GETUDPRELAY)cmd;
        }

        public override void DataOperation()
        {
            base.DataOperation();
        }

        public override void ConstructResponse()
        {
            base.ConstructResponse();
        }

    }
}
