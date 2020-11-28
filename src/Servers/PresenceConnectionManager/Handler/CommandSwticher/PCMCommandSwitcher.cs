using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

using PresenceConnectionManager.Entity.Structure;
using System;
using PresenceConnectionManager.Handler.CommandHandler;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    public class PCMCommandSwitcher
    {
        public static void Switch(ISession session, string rawRequest)
        {

            var requests = PCMRequestSerializer.Serialize(session, rawRequest);

            foreach (var requst in requests)
            {
                switch (requst.CommandName)
                {
                    case PCMRequestName.Login://login to retrospy
                        new LoginHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.Logout://logout from retrospy
                        new LogoutHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.KeepAlive:
                        new KeepAliveHandler(session, requst).Handle();
                        break;

                    case PCMRequestName.GetProfile://get profile of a player
                        new GetProfileHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.RegisterNick://update user's uniquenick
                        new RegisterNickHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.NewUser://create an new user
                        new NewUserHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.UpdateUI://update a user's email
                        new UpdateUIHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.UpdatePro://update a user's profile
                        new UpdateProHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.NewProfile://create an new profile
                        new NewProfileHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.DelProfile://delete profile
                        break;
                    case PCMRequestName.AddBlock://add an user to our block list
                        new AddBlockHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.RemoveBlock:
                        new RemoveBlockHandler(session, requst).Handle();
                        break;

                    case PCMRequestName.AddBuddy://Send a request which adds an user to our friend list
                        new AddBuddyHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.DelBuddy://delete a user from our friend list
                        new DelBuddyHandler(session, requst).Handle();
                        break;
                    case PCMRequestName.Status://update current logged in user's status info
                        new StatusHandler(session, requst).Handle();
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
    }
}