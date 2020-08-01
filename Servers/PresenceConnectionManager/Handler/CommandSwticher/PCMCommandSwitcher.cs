using System;
using System.Collections.Generic;
using System.Linq;
using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceConnectionManager.Handler.CommandHandler.Buddy;
using PresenceConnectionManager.Handler.CommandHandler.General;
using PresenceConnectionManager.Handler.CommandHandler.Profile;
using PresenceSearchPlayer.Handler.CommandHandler.NewUser;
using Serilog.Events;

namespace PresenceConnectionManager.Handler
{
    public class PCMCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, string message)
        {
            try
            {
                message = ((PCMSession)session.GetInstance()).RequstFormatConversion(message);
                if (message[0] != '\\')
                {
                    LogWriter.ToLog(LogEventLevel.Error, "Invalid request recieved!");
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
                        #region General handler
                        case "login"://login to retrospy
                            new LoginHandler(session, recv).Handle();
                            break;

                        case "logout"://logout from retrospy
                            new LogoutHandler(session, recv).Handle();
                            break;
                        case "ka":
                            new KeepAliveHandler(session, recv).Handle();
                            break;
                        #endregion

                        #region Profile handler
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

                        #endregion

                        #region Buddy handler
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
                        #endregion
                        default:
                            LogWriter.UnknownDataRecieved(message);
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                LogWriter.ToLog(LogEventLevel.Error, e.ToString());
            }
        }
    }
}
