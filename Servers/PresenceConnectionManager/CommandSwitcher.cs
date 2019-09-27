using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceConnectionManager.Application;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager
{
    public class CommandSwitcher
    {
        public static void Switch(GPCMClient client, Dictionary<string, string> recv, GPCMConnectionUpdate OnSuccessfulLogin, GPCMStatusChanged OnStatusChanged)
        {
            string command = recv.Keys.First();
            try
            {
                switch (command)
                {
                    case "inviteto":
                        InviteToHandler.AddFriends(client, recv);
                        break;
                    case "login":
                        LoginHandler.ProcessLogin(client, recv, OnSuccessfulLogin, OnStatusChanged);
                        break;
                    case "getprofile":
                        GetProfileHandler.SendProfile(client, recv);
                        break;
                    case "addbuddy":
                        AddBuddyHandler.Addfriends(client, recv);
                        break;
                    case "delbuddy":
                        DelBuddyHandler.Handle(client, recv);
                        break;
                    case "updateui":
                        UpdateUiHandler.UpdateUi(client, recv);
                        break;
                    case "updatepro":
                        UpdateProHandler.UpdateUser(client, recv);
                        break;
                    case "registernick":
                        RegisterNickHandler.RegisterNick(client, recv);
                        break;
                    case "logout":
                        client.Disconnect(DisconnectReason.NormalLogout);
                        break;
                    case "status":
                        StatusHandler.UpdateStatus(client, recv, OnStatusChanged);
                        break;
                    case "newuser":
                        NewUserHandler.NewUser(client, recv);
                        break;
                    case "ka":
                        KAHandler.SendKeepAlive(client);
                        break;
                    default:
                        LogWriter.Log.Write("[GPCM] received unknown data " + command, LogLevel.Debug);
                        GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "An invalid request was sended.");
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
