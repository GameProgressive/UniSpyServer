using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

using PresenceConnectionManager.Entity.Structure;
using System;
using PresenceConnectionManager.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    public class PCMCommandSwitcher : CommandSerializerBase
    {
        protected new string _rawRequest;
        public PCMCommandSwitcher(ISession session, object rawRequest) : base(session, rawRequest)
        {
        }

        public override void Serialize()
        {
            var requests = PCMRequestSerializer.Serialize(_session, _rawRequest);

            foreach (var requst in requests)
            {
                switch (requst.CommandName)
                {
                    case PCMRequestName.Login://login to retrospy
                        new LoginHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.Logout://logout from retrospy
                        new LogoutHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.KeepAlive:
                        new KeepAliveHandler(_session, requst).Handle();
                        break;

                    case PCMRequestName.GetProfile://get profile of a player
                        new GetProfileHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.RegisterNick://update user's uniquenick
                        new RegisterNickHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.NewUser://create an new user
                        new NewUserHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.UpdateUI://update a user's email
                        new UpdateUIHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.UpdatePro://update a user's profile
                        new UpdateProHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.NewProfile://create an new profile
                        new NewProfileHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.DelProfile://delete profile
                        break;
                    case PCMRequestName.AddBlock://add an user to our block list
                        new AddBlockHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.RemoveBlock:
                        new RemoveBlockHandler(_session, requst).Handle();
                        break;

                    case PCMRequestName.AddBuddy://Send a request which adds an user to our friend list
                        new AddBuddyHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.DelBuddy://delete a user from our friend list
                        new DelBuddyHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.Status://update current logged in user's status info
                        new StatusHandler(_session, requst).Handle();
                        break;
                    case PCMRequestName.StatusInfo:
                        throw new NotImplementedException();
                    case PCMRequestName.InviteTo:
                        throw new NotImplementedException();
                    default:
                        LogWriter.UnknownDataRecieved(_rawRequest);
                        break;
                }
            }
        }
    }
}