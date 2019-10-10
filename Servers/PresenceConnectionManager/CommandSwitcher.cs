using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager
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
                        InviteToHandler.AddFriends(session, recv);
                        break;
                    case "login":
                        LoginHandler.ProcessLogin(session, recv);
                        break;
                    case "getprofile":
                        GetProfileHandler.SendProfile(session, recv);
                        break;
                    case "addbuddy":
                        AddBuddyHandler.Addfriends(session, recv);
                        break;
                    case "delbuddy":
                        DelBuddyHandler.Handle(session, recv);
                        break;
                    case "updateui":
                        UpdateUiHandler.UpdateUi(session, recv);
                        break;
                    case "updatepro":
                        UpdateProHandler.UpdateUser(session, recv);
                        break;
                    case "registernick":
                        RegisterNickHandler.RegisterNick(session, recv);
                        break;
                    case "logout":
                        session.DisconnectByReason(DisconnectReason.NormalLogout);
                        break;
                    case "status":
                        StatusHandler.UpdateStatus(session, recv);
                        break;
                    case "newuser":
                        NewUserHandler.NewUser(session, recv);
                        break;
                    case "ka":
                        KAHandler.SendKeepAlive(session);
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
