using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Logging;
using UniSpyLib.MiscMethod;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Entity.Structure.Request.Buddy;
using PresenceConnectionManager.Entity.Structure.Request.General;
using PresenceConnectionManager.Entity.Structure.Request.Profile;
using PresenceSearchPlayer.Entity.Structure.Request;
using System;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceConnectionManager.Handler.CommandSwitcher
{
    public class PCMRequestSerializer : RequestSerializerBase
    {
        protected new string _rawRequest;

        public PCMRequestSerializer(object rawRequest) : base(rawRequest)
        {
            _rawRequest = (string)rawRequest;
        }

        public override IRequest Serialize()
        {
            // Read client message, and parse it into key value pairs
            var keyValue = GameSpyUtils.ConvertToKeyValue(_rawRequest);

            switch (keyValue.Keys.First())
            {
                case PCMRequestName.Login:
                    return new LoginRequest(keyValue);
                case PCMRequestName.Logout:
                    return new LogoutRequest(keyValue);
                case PCMRequestName.KeepAlive:
                    return new KeepAliveRequest(keyValue);
                case PCMRequestName.GetProfile:
                    return new GetProfileRequest(keyValue);
                case PCMRequestName.RegisterNick:
                    return new RegisterNickRequest(keyValue);
                case PCMRequestName.NewUser:
                    return new NewUserRequest(keyValue);
                case PCMRequestName.UpdateUI:
                    return new UpdateUIRequest(keyValue);
                case PCMRequestName.UpdatePro:
                    return new UpdateProRequest(keyValue);
                case PCMRequestName.NewProfile:
                    return new NewUserRequest(keyValue);
                case PCMRequestName.DelProfile:
                    throw new NotImplementedException();
                case PCMRequestName.AddBlock:
                    return new AddBlockRequest(keyValue);
                case PCMRequestName.RemoveBlock:
                    throw new NotImplementedException();
                case PCMRequestName.AddBuddy:
                    return new AddBuddyRequest(keyValue);
                case PCMRequestName.DelBuddy:
                    return new DelBuddyRequest(keyValue);
                case PCMRequestName.Status:
                    return new StatusRequest(keyValue);
                case PCMRequestName.StatusInfo:
                    return new StatusInfoRequest(keyValue);
                case PCMRequestName.InviteTo:
                    return new InviteToRequest(keyValue);
                default:
                    LogWriter.UnknownDataRecieved(_rawRequest);
                    return null;
            }
        }
    }
}
