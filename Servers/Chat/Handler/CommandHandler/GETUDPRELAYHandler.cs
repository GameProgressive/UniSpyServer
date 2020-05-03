using System;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    /// <summary>
    /// currently we do not know how this work
    /// so we do not implement it
    /// </summary>
    public class GETUDPRELAYHandler:ChatCommandHandlerBase
    {
        public GETUDPRELAYHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
        }
    }
}
