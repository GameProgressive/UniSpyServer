using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Buddy.AddBlock;
using PresenceConnectionManager.Handler.Buddy.AddBuddy;
using PresenceConnectionManager.Handler.Buddy.DelBuddy;
using PresenceConnectionManager.Handler.CommandHandler.Buddy.Status;
using PresenceConnectionManager.Handler.General.Login.LoginMethod;
using PresenceConnectionManager.Handler.Profile.GetProfile;
using PresenceConnectionManager.Handler.Profile.NewProfile;
using PresenceConnectionManager.Handler.Profile.NewUser;
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
        public static void Switch(GPCMSession session, string message)
        {
            try
            {
                message = session.RequstFormatConversion(message);
                if (message[0] != '\\')
                {
                    GameSpyUtils.SendGPError(session, GPErrorCode.General, "An invalid request was sended.");
                    return;
                }
                string[] commands = message.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);

                foreach (string command in commands)
                {
                    if (command.Length < 1)
                    {
                        continue;
                    }

                    // Read client message, and parse it into key value pairs
                    string[] recieved = command.TrimStart('\\').Split('\\');

                    Dictionary<string, string> recv = GameSpyUtils.ConvertRequestToKeyValue(recieved);

                    switch (recv.Keys.First())
                    {
                        //case "inviteto":
                        //    InviteToHandler.InvitePlayer(session, recv);
                        //    break;

                        case "login"://login to retrospy
                            new LoginHandler().Handle(session, recv);
                            break;

                        case "getprofile"://get profile of a player
                            new GetProfileHandler().Handle(session, recv);
                            break;

                        case "addbuddy"://Send a request which adds an user to our friend list
                            new AddBuddyHandler().Handle(session, recv);
                            break;

                        case "delbuddy"://delete a user from our friend list
                            new DelBuddyHandler().Handle(session, recv);
                            break;

                        case "updateui"://update a user's email
                            new UpdateUIHandler().Handle(session, recv);
                            break;

                        case "updatepro"://update a user's profile
                            new UpdateProHandler().Handle(session, recv);
                            break;

                        case "registernick"://update user's uniquenick
                            new RegisterNickHandler().Handle(session, recv);
                            break;

                        case "logout"://logout from retrospy
                            session.Disconnect();
                            GPCMServer.LoggedInSession.TryRemove(session.Id, out _);
                            break;

                        case "status"://update current logged in user's status info
                            new StatusHandler().Handle(session, recv);
                            break;

                        case "newuser"://create an new user
                            new NewUserHandler().Handle(session, recv);
                            break;

                        case "addblock"://add an user to our block list
                            new AddBlockHandler().Handle(session, recv);
                            break;

                        case "ka":
                            //KAHandler.SendKeepAlive(session);
                            break;

                        case "newprofile"://create an new profile
                            new NewProfileHandler().Handle(session, recv);
                            break;

                        default:
                            session.UnknownDataReceived(message);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, e.ToString());
            }
        }
    }
}
