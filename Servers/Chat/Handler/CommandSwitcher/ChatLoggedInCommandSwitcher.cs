using System;
using Chat.Entity.Structure.ChatCommand;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;

namespace Chat.Handler.CommandSwitcher
{
    public class ChatLoggedInCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, ChatRequestBase request)
        {
            switch(request.CmdName)
            {


            }
        }
    }
}
