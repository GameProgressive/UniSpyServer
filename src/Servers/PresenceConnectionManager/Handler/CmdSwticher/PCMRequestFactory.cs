using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Structure.Request;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    internal class PCMRequestFactory : UniSpyRequestFactoryBase
    {
        private new string _rawRequest => (string)base._rawRequest;

        public PCMRequestFactory(object rawRequest) : base(rawRequest)
        {
        }

        public override IUniSpyRequest Serialize()
        {
            // Read client message, and parse it into key value pairs
            var keyValues = GameSpyUtils.ConvertToKeyValue(_rawRequest);

            switch (keyValues.Keys.First())
            {
                case PCMRequestName.Login:
                    return new LoginRequest(_rawRequest);
                case PCMRequestName.Logout:
                    return new LogoutRequest(_rawRequest);
                case PCMRequestName.KeepAlive:
                    return new KeepAliveRequest(_rawRequest);
                case PCMRequestName.GetProfile:
                    return new GetProfileRequest(_rawRequest);
                case PCMRequestName.RegisterNick:
                    return new RegisterNickRequest(_rawRequest);
                case PCMRequestName.NewUser:
                    return new NewUserRequest(_rawRequest);
                case PCMRequestName.UpdateUI:
                    return new UpdateUIRequest(_rawRequest);
                case PCMRequestName.UpdatePro:
                    return new UpdateProRequest(_rawRequest);
                case PCMRequestName.NewProfile:
                    return new NewUserRequest(_rawRequest);
                case PCMRequestName.DelProfile:
                    throw new NotImplementedException();
                case PCMRequestName.AddBlock:
                    return new AddBlockRequest(_rawRequest);
                case PCMRequestName.RemoveBlock:
                    throw new NotImplementedException();
                case PCMRequestName.AddBuddy:
                    return new AddBuddyRequest(_rawRequest);
                case PCMRequestName.DelBuddy:
                    return new DelBuddyRequest(_rawRequest);
                case PCMRequestName.Status:
                    return new StatusRequest(_rawRequest);
                case PCMRequestName.StatusInfo:
                    return new StatusInfoRequest(_rawRequest);
                case PCMRequestName.InviteTo:
                    return new InviteToRequest(_rawRequest);
                default:
                    LogWriter.UnknownDataRecieved(_rawRequest);
                    return null;
            }
        }
    }
}
