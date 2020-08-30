using Chat.Entity.Structure;
using Chat.Entity.Structure.ChatCommand;
using Chat.Handler.CommandHandler.ChatGeneralCommandHandler;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandSwitcher
{
    public class ChatGeneralCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, ChatRequestBase request)
        {
            switch(request.CmdName)
            {
                case ChatCmdName.CRYPT:
                    new CRYPTHandler(session, request).Handle();
                    break;
                case ChatCmdName.LOGIN:
                    new LOGINHandler(session, request).Handle();
                    break;
                case ChatCmdName.NICK:
                    new NICKHandler(session, request).Handle();
                    break;
            }
        }
    }
}
