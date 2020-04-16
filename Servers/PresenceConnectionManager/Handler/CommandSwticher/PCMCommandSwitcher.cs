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
using PresenceSearchPlayer.Handler.CommandHandler.NewUser;
using PresenceConnectionManager.Handler.Profile.RegisterNick;
using PresenceConnectionManager.Handler.Profile.UpdatePro;
using PresenceConnectionManager.Handler.Profile.UpdateUI;
using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.BaseClass;

namespace PresenceConnectionManager.Handler
{
    public class PCMCommandSwitcher:CommandSwitcherBase
    {
        public void Switch(GPCMSession session, string message)
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
                        //    InviteToHandler.InvitePlayer();
                        //    break;

                        case "login"://login to retrospy
                            new LoginHandler(session, recv).Handle();
                            break;

                        case "getprofile"://get profile of a player
                            new GetProfileHandler(session, recv).Handle();
                            break;

                        case "addbuddy"://Send a request which adds an user to our friend list
                            new AddBuddyHandler(session, recv).Handle();
                            break;

                        case "delbuddy"://delete a user from our friend list
                            new DelBuddyHandler(session, recv).Handle();
                            break;

                        case "updateui"://update a user's email
                            new UpdateUIHandler(session, recv).Handle();
                            break;

                        case "updatepro"://update a user's profile
                            new UpdateProHandler(session, recv).Handle();
                            break;

                        case "registernick"://update user's uniquenick
                            new RegisterNickHandler(session, recv).Handle();
                            break;

                        case "logout"://logout from retrospy
                            session.Disconnect();
                            GPCMServer.LoggedInSession.TryRemove(session.Id, out _);
                            break;

                        case "status"://update current logged in user's status info
                            new StatusHandler(session, recv).Handle();
                            break;

                        case "newuser"://create an new user
                            new NewUserHandler(session, recv).Handle();
                            break;

                        case "addblock"://add an user to our block list
                            new AddBlockHandler(session, recv).Handle();
                            break;

                        case "ka":
                            //KAHandler.SendKeepAlive(_session);
                            break;

                        case "newprofile"://create an new profile
                            new NewProfileHandler(session, recv).Handle();
                            break;

                        default:
                            LogWriter.UnknownDataRecieved(message);
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
