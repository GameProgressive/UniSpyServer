using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using PresenceConnectionManager.Handler.CommandHandler.Buddy;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.CommandSwticher
{
    public class BuddyCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, Dictionary<string, string> recv)
        {
            switch (recv.Keys.First())
            {
                case "addbuddy"://Send a request which adds an user to our friend list
                    new AddBuddyHandler(session, recv).Handle();
                    break;
                case "delbuddy"://delete a user from our friend list
                    new DelBuddyHandler(session, recv).Handle();
                    break;
                case "status"://update current logged in user's status info
                    new StatusHandler(session, recv).Handle();
                    break;
                case "statusinfo":
                    throw new NotImplementedException();
                    //case "inviteto":
                    //    InviteToHandler.InvitePlayer();
                    //    break;
            }
        }
    }
}
