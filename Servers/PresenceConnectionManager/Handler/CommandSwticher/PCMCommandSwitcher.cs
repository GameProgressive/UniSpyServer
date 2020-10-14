using GameSpyLib.Common.BaseClass;
using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Logging;
using GameSpyLib.MiscMethod;
using PresenceConnectionManager.Entity.BaseClass;
using PresenceConnectionManager.Handler.CommandSwticher;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler
{
    public class PCMCommandSwitcher : CommandSwitcherBase
    {
        public void Switch(ISession session, string message)
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
                        case "login"://login to retrospy
                        case "logout"://logout from retrospy
                        case "ka":
                            new GeneralCommandSwitcher().Switch(session, recv);
                            break;

                        case "getprofile"://get profile of a player
                        case "registernick"://update user's uniquenick
                        case "newuser"://create an new user
                        case "updateui"://update a user's email
                        case "updatepro"://update a user's profile
                        case "newprofile"://create an new profile
                        case "delprofile"://delete profile
                        case "addblock"://add an user to our block list
                        case "removeblock":
                            new ProfileCommandSwitcher().Switch(session, recv);
                            break;

                        case "addbuddy"://Send a request which adds an user to our friend list
                        case "delbuddy"://delete a user from our friend list
                        case "status"://update current logged in user's status info
                        case "statusinfo":
                        case "inviteto":
                            new BuddyCommandSwitcher().Switch(session, recv);
                            break;

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