using System;
using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandHandler
{
    public class ChatLogedInHandlerBase : ChatCommandHandlerBase
    {
        public ChatLogedInHandlerBase(ISession session, ChatCommandBase cmd) : base(session, cmd)
        {
        }

        public override void Handle()
        {
            if (!_session.UserInfo.IsLogedIn)
            {
                return;
            }

            base.Handle();
        }
    }
}
