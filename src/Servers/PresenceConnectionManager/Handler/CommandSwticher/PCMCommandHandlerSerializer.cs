using System;
using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Handler.CommandHandler;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;

namespace PresenceConnectionManager.Handler.CommandSwticher
{
    public class PCMCommandHandlerSerializer : UniSpyCmdHandlerSerializerBase
    {
        protected new PCMRequestBase _request;
        public PCMCommandHandlerSerializer(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _request = (PCMRequestBase)request;
        }

        public override IUniSpyHandler Serialize()
        {
            switch (_request.CommandName)
            {
                case PCMRequestName.Login://login to retrospy
                    return new LoginHandler(_session, _request);
                case PCMRequestName.Logout://logout from retrospy
                    return new LogoutHandler(_session, _request);
                case PCMRequestName.KeepAlive:
                    return new KeepAliveHandler(_session, _request);
                case PCMRequestName.GetProfile://get profile of a player
                    return new GetProfileHandler(_session, _request);
                case PCMRequestName.RegisterNick://update user's uniquenick
                    return new RegisterNickHandler(_session, _request);
                case PCMRequestName.NewUser://create an new user
                    return new NewUserHandler(_session, _request);
                case PCMRequestName.UpdateUI://update a user's email
                    return new UpdateUIHandler(_session, _request);
                case PCMRequestName.UpdatePro://update a user's profile
                    return new UpdateProHandler(_session, _request);
                case PCMRequestName.NewProfile://create an new profile
                    return new NewProfileHandler(_session, _request);
                case PCMRequestName.DelProfile://delete profile
                    throw new NotImplementedException();
                case PCMRequestName.AddBlock://add an user to our block list
                    return new AddBlockHandler(_session, _request);
                case PCMRequestName.RemoveBlock:
                    return new RemoveBlockHandler(_session, _request);
                case PCMRequestName.AddBuddy://Send a request which adds an user to our friend list
                    return new AddBuddyHandler(_session, _request);
                case PCMRequestName.DelBuddy://delete a user from our friend list
                    return new DelBuddyHandler(_session, _request);
                case PCMRequestName.Status://update current logged in user's status info
                    return new StatusHandler(_session, _request);
                case PCMRequestName.StatusInfo:
                    throw new NotImplementedException();
                case PCMRequestName.InviteTo:
                    throw new NotImplementedException();
                default:
                    LogWriter.UnknownDataRecieved("");
                    throw new Exception();
            }
        }
    }
}
