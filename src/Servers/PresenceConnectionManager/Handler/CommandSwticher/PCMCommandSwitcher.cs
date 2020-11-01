using GameSpyLib.Abstraction.Interface;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Abstraction.BaseClass.Buddy;
using PresenceConnectionManager.Abstraction.BaseClass.General;
using PresenceConnectionManager.Abstraction.BaseClass.Profile;
using PresenceConnectionManager.Entity.Structure;
using PresenceSearchPlayer.Handler.CommandHandler.NewUser;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Abstraction
{
    public class PCMCommandSwitcher
    {
        public static void Switch(ISession session, string rawRequest)
        {
            try
            {
                //message = @"\login\\challenge\VPUKQ5CiXSqtt0EdOKMwwRvf3CHqxrah\user\borger@mike@vale.ski\partnerid\0\response\4ec2535ddba4773168337c7b5f9588e7\firewall\1\port\0\productid\10936\gamename\greconawf2\namespaceid\0\sdkrevision\3\id\1\final\";
                rawRequest = PCMRequestBase.NormalizeRequest(rawRequest);
                if (rawRequest[0] != '\\')
                {
                    LogWriter.ToLog(LogEventLevel.Error, "Invalid request recieved!");
                    return;
                }
                string[] commands = rawRequest.Split("\\final\\", StringSplitOptions.RemoveEmptyEntries);

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
                        case PCMRequestName.Login://login to retrospy
                            new LoginHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.Logout://logout from retrospy
                            new LogoutHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.KeepAlive:
                            new KeepAliveHandler(session, recv).Handle();
                            break;

                        case PCMRequestName.GetProfile://get profile of a player
                            new GetProfileHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.RegisterNick://update user's uniquenick
                            new RegisterNickHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.NewUser://create an new user
                            new NewUserHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.UpdateUI://update a user's email
                            new UpdateUIHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.UpdatePro://update a user's profile
                            new UpdateProHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.NewProfile://create an new profile
                            new NewProfileHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.DelProfile://delete profile
                            break;
                        case PCMRequestName.AddBlock://add an user to our block list
                            new AddBlockHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.RemoveBlock:
                            new RemoveBlockHandler(session, recv).Handle();
                            break;

                        case PCMRequestName.AddBuddy://Send a request which adds an user to our friend list
                            new AddBuddyHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.DelBuddy://delete a user from our friend list
                            new DelBuddyHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.Status://update current logged in user's status info
                            new StatusHandler(session, recv).Handle();
                            break;
                        case PCMRequestName.StatusInfo:
                            throw new NotImplementedException();
                        case PCMRequestName.InviteTo:
                            throw new NotImplementedException();
                        default:
                            LogWriter.UnknownDataRecieved(rawRequest);
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