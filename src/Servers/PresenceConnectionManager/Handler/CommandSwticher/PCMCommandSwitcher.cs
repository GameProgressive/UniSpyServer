using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Handler.CommandHandler.Buddy;
using PresenceConnectionManager.Handler.CommandHandler.General;
using PresenceConnectionManager.Handler.CommandHandler.Profile;
using PresenceSearchPlayer.Handler.CommandHandler.NewUser;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler
{
    public class PCMCommandSwitcher
    {
        public static void Switch(ISession session, string message)
        {
            try
            {
                //message = @"\login\\challenge\VPUKQ5CiXSqtt0EdOKMwwRvf3CHqxrah\user\borger@mike@vale.ski\partnerid\0\response\4ec2535ddba4773168337c7b5f9588e7\firewall\1\port\0\productid\10936\gamename\greconawf2\namespaceid\0\sdkrevision\3\id\1\final\";
                message = PCMRequest.NormalizeRequest(message);
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
                        case PCMRequestName.LogIn://login to retrospy
                            new LoginHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.LogOut://logout from retrospy
                            new LogoutHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.KeepAlive:
                            new KeepAliveHandler(session, recv).Handle();
                            break;

                        case PCMRequestName.GetPlayerProfile://get profile of a player
                            new GetProfileHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.RegisterNickName://update user's uniquenick
                            new RegisterNickHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.CreateNewUser://create an new user
                            new NewUserHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.UpdateUserInformation://update a user's email
                            new UpdateUIHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.UpdateUserProfile://update a user's profile
                            new UpdateProHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.CreateNewProfile://create an new profile
                            new NewProfileHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.DeleteProfile://delete profile
                            break;
                        case PCMRequestName.AddUserToBlockList://add an user to our block list
                            new AddBlockHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.RemoveUserFromBlockList:
                            new RemoveBlockHandler(session, recv).Handle();
                            break;

                        case PCMRequestName.AddUserToFriendList://Send a request which adds an user to our friend list
                            new AddBuddyHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.RemoveUserFromFriendList://delete a user from our friend list
                            new DelBuddyHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.UpdateUserStatus://update current logged in user's status info
                            new StatusHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.UpdateUserStatusInformation:
                            throw new NotImplementedException();
                        case PCMRequestName.InviteUserToGame:
                            throw new NotImplementedException();
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