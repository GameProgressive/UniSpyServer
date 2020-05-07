using System;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler.ChatBasicCommandHandler
{
    public class WHOHandler : ChatCommandHandlerBase
    {
        public WHOHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
        }
    }
}
