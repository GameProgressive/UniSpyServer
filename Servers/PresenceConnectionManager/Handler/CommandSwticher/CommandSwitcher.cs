using GameSpyLib.Logging;
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
using PresenceConnectionManager.Handler.Profile.NewUser;
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
                    case "login"://login to retrospy
                        LoginHandler login = new LoginHandler(session, recv);
                        break;
                    case "getprofile"://get profile of a player
                        GetProfileHandler get = new GetProfileHandler(session, recv);
                        break;
                    case "addbuddy"://Send a request which adds an user to our friend list
                        AddBuddyHandler add = new AddBuddyHandler(session, recv);
                        break;
                    case "delbuddy"://delete a user from our friend list
                        DelBuddyHandler delBuddy = new DelBuddyHandler(session, recv);
                        break;
                    case "updateui"://update a user's email
                        UpdateUIHandler updateUI = new UpdateUIHandler(session, recv);
                        break;
                    case "updatepro"://update a user's profile
                        UpdateProHandler updatePro = new UpdateProHandler(session, recv);
                        break;
                    case "registernick"://update user's uniquenick
                        RegisterNickHandler register = new RegisterNickHandler(session, recv);
                        break;
                    case "logout"://logout from retrospy
                        session.Disconnect();
                        GPCMServer.LoggedInSession.TryRemove(session.Id, out _);
                        break;
                    case "status"://update current logged in user's status info
                        StatusHandler status = new StatusHandler(session, recv);
                        break;
                    case "newuser"://create an new user
                        NewUserHandler newUser = new NewUserHandler(session, recv);
                        break;
                    case "addblock"://add an user to our block list
                        AddBlockHandler addBlock = new AddBlockHandler(session, recv);
                        break;
                    case "ka":
                        //KAHandler.SendKeepAlive(session);
                        break;
                    case "newprofile"://create an new profile
                        NewProfileHandler newProfile = new NewProfileHandler(session, recv);
                        break;
                    default:
                        session.UnknownDataReceived(recv);
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
