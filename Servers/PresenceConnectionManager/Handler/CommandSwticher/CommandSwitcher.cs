using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Buddy.AddBlock;
using PresenceConnectionManager.Handler.Buddy.AddBuddy;
using PresenceConnectionManager.Handler.Buddy.DelBuddy;
using PresenceConnectionManager.Handler.CommandHandler.Buddy.Status;
using PresenceConnectionManager.Handler.General.Login.LoginMethod;
using PresenceConnectionManager.Handler.Profile.GetProfile;
using PresenceConnectionManager.Handler.Profile.NewProfile;
using PresenceConnectionManager.Handler.Profile.RegisterNick;
using PresenceConnectionManager.Handler.Profile.UpdatePro;
using PresenceConnectionManager.Handler.Profile.UpdateUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler
{
    public class CommandSwitcher
    {
        public static void Switch(GPCMSession session, Dictionary<string, string> recv)
        {
            string command = recv.Keys.First();
            try
            {
                switch (command)
                {
                    //case "inviteto":
                    //    InviteToHandler.InvitePlayer(session, recv);
                    //    break;
                    case "login"://login to gamespy
                        LoginHandler login = new LoginHandler(recv);
                        login.Handle(session); 
                        break;
                    case "getprofile"://get profile of a player
                        GetProfileHandler get = new GetProfileHandler(recv);
                        get.Handle(session);
                        break;
                    case "addbuddy"://Send a request which adds an user to our friend list
                        AddBuddyHandler add = new AddBuddyHandler(recv);
                        add.Handle(session);
                        break;
                    case "delbuddy"://delete a user from our friend list
                        DelBuddyHandler delBuddy = new DelBuddyHandler(recv);
                        delBuddy.Handle(session);
                        break;
                    case "updateui"://update a user's email
                        UpdateUIHandler.UpdateUI(session, recv);
                        break;
                    case "updatepro"://update a user's profile
                        UpdateProHandler updatePro = new UpdateProHandler(recv);
                        updatePro.Handle(session);
                        break;
                    case "registernick"://update user's uniquenick
                        RegisterNickHandler register = new RegisterNickHandler(recv);
                        register.Handle(session);
                        break;
                    case "logout":
                        session.DisconnectByReason(DisconnectReason.NormalLogout);
                        break;
                    case "status"://update current logged in user's status info
                        StatusHandler status = new StatusHandler(recv);
                        status.Handle(session);
                        break;
                    case "newuser"://create an new user
                        break;
                    case "addblock"://add an user to our block list
                        AddBlockHandler addBlock = new AddBlockHandler(recv);
                        addBlock.Handle(session);
                        break;
                    case "ka":
                        //KAHandler.SendKeepAlive(session);
                        break;
                    case "newprofile":
                        NewProfileHandler newProfile = new NewProfileHandler(recv);
                        newProfile.Handle(session);
                        break;
                    default:
                        session.UnknownDataRecived(recv);
                        break;
                }
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }

    }
}
