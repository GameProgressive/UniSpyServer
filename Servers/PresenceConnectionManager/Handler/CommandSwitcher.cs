using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.AddBlock;
using PresenceConnectionManager.Handler.AddBuddy;
using PresenceConnectionManager.Handler.DelBuddy;
using PresenceConnectionManager.Handler.GetProfile;
using PresenceConnectionManager.Handler.InviteTo;
using PresenceConnectionManager.Handler.KeepAlive;
using PresenceConnectionManager.Handler.Login;
using PresenceConnectionManager.Handler.NewUser;
using PresenceConnectionManager.Handler.RegisterNick;
using PresenceConnectionManager.Handler.Status;
using PresenceConnectionManager.Handler.UpdatePro;
using PresenceConnectionManager.Handler.UpdateUI;
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
                    case "inviteto":
                        InviteToHandler.InvitePlayer(session, recv);
                        break;
                    case "login"://login to gamespy
                        LoginHandler.ProcessLogin(session, recv);
                        break;
                    case "getprofile"://get profile of a player
                        GetProfileHandler.SendProfile(session, recv);
                        break;
                    case "addbuddy"://Send a request which adds an user to our friend list
                        AddBuddyHandler.Addfriends(session, recv);
                        break;
                    case "delbuddy"://delete a user from our friend list
                        DelBuddyHandler.Handle(session, recv);
                        break;
                    case "updateui"://update a user's email
                        UpdateUIHandler.UpdateUI(session, recv);
                        break;
                    case "updatepro"://update a user's profile
                        UpdateProHandler.UpdateUser(session, recv);
                        break;
                    case "registernick"://update user's uniquenick
                        RegisterNickHandler.RegisterNick(session, recv);
                        break;
                    case "logout":
                        session.DisconnectByReason(DisconnectReason.NormalLogout);
                        break;
                    case "status"://update current logged in user's status info
                        StatusHandler.UpdateStatus(session, recv);
                        break;
                    case "newuser"://create an new user
                        NewUserHandler.NewUser(session, recv);
                        break;
                    case "ka"://keep alive
                        KAHandler.SendKeepAlive(session);
                        break;
                    case "addblock"://add an user to our block list
                        AddBlockHandler.AddUserToBlockList(session, recv);
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
