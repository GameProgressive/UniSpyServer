using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Handler.CommandHandler.General;

namespace PresenceConnectionManager.Handler.CommandSwticher
{
    public class GeneralCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, Dictionary<string, string> recv)
        {
            switch (recv.Keys.First())
            {
                case "login"://login to retrospy
                    new LoginHandler(session, recv).Handle();
                    break;
                case "logout"://logout from retrospy
                    new LogoutHandler(session, recv).Handle();
                    break;
                case "ka":
                    new KeepAliveHandler(session, recv).Handle();
                    break;
            }
        }
    }
}
