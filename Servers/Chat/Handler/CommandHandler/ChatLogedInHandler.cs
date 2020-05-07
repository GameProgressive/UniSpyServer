using System;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class ChatLogedInHandler : ChatCommandHandlerBase
    {
        public ChatLogedInHandler(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
        }
    }
}
