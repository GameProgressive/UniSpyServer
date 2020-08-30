using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using PresenceConnectionManager.Handler.CommandHandler.Profile;
using PresenceSearchPlayer.Handler.CommandHandler.NewUser;

namespace PresenceConnectionManager.Handler.CommandSwticher
{
    public class ProfileCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, Dictionary<string, string> recv)
        {
            switch (recv.Keys.First())
            {
                case "getprofile"://get profile of a player
                    new GetProfileHandler(session, recv).Handle();
                    break;
                case "registernick"://update user's uniquenick
                    new RegisterNickHandler(session, recv).Handle();
                    break;
                case "newuser"://create an new user
                    new NewUserHandler(session, recv).Handle();
                    break;
                case "updateui"://update a user's email
                    new UpdateUIHandler(session, recv).Handle();
                    break;
                case "updatepro"://update a user's profile
                    new UpdateProHandler(session, recv).Handle();
                    break;
                case "newprofile"://create an new profile
                    new NewProfileHandler(session, recv).Handle();
                    break;
                case "delprofile"://delete profile
                    break;
                case "addblock"://add an user to our block list
                    new AddBlockHandler(session, recv).Handle();
                    break;
                case "removeblock":
                    new RemoveBlockHandler(session, recv).Handle();
                    break;
            }
        }
    }
}
