using System;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Buddy;
using UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.General;
using UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.Profile;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.Abstraction.BaseClass;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Encryption;
using UniSpy.Server.PresenceSearchPlayer.Contract.Request;

namespace UniSpy.Server.PresenceConnectionManager.Handler
{
    public sealed class CmdSwitcher : CmdSwitcherBase
    {
        private new string _rawRequest => UniSpyEncoding.GetString((byte[])base._rawRequest);
        public CmdSwitcher(IClient client, object rawRequest) : base(client, rawRequest)
        {
        }
        protected override void ProcessRawRequest()
        {
            if (_rawRequest[0] != '\\')
            {
                throw new GPParseException("Request format is invalid");
            }
            var rawRequests = _rawRequest.Split(@"\final\", StringSplitOptions.RemoveEmptyEntries);

            foreach (var rawRequest in rawRequests)
            {
                var name = rawRequest.TrimStart('\\').Split('\\').First();
                _requests.Add(new KeyValuePair<object, object>(name, rawRequest));
            }
        }

        protected override IHandler CreateCmdHandlers(object name, object rawRequest)
        {
            switch ((string)name)
            {
                # region General
                case "ka":
                    return new KeepAliveHandler(_client, new KeepAliveRequest((string)rawRequest));
                case "login":
                    return new LoginHandler(_client, new LoginRequest((string)rawRequest));
                case "logout":
                    return new LogoutHandler(_client, new LogoutRequest((string)rawRequest));
                case "newuser":
                    return new NewUserHandler(_client, new NewUserRequest((string)rawRequest));
                #endregion
                # region Profile
                case "addblock":
                    return new AddBlockHandler(_client, new AddBlockRequest((string)rawRequest));
                case "getprofile":
                    return new GetProfileHandler(_client, new GetProfileRequest((string)rawRequest));
                case "newprofile":
                    return new NewProfileHandler(_client, new NewProfileRequest((string)rawRequest));
                case "registercdkey":
                    return new RegisterCDKeyHandler(_client, new RegisterCDKeyRequest((string)rawRequest));
                case "registernick":
                    return new RegisterNickHandler(_client, new RegisterNickRequest((string)rawRequest));
                case "updatepro":
                    return new UpdateProfileHandler(_client, new UpdateProfileRequest((string)rawRequest));
                case "updateui":
                    // return new UpdateUserInfoHandler(_client, new UpdateUserInfoRequest((string)rawRequest));
                    throw new NotImplementedException();

                # endregion
                # region Buddy
                case "addbuddy":
                    return new AddBuddyHandler(_client, new AddBuddyRequest((string)rawRequest));
                case "delbuddy":
                    return new DelBuddyHandler(_client, new DelBuddyRequest((string)rawRequest));
                case "bsi":
                    // return new BuddyStatusInfoHandler(_client, new BuddyStatusInfoRequest((string)rawRequest));
                    throw new NotImplementedException();
                case "status":
                    return new StatusHandler(_client, new StatusRequest((string)rawRequest));
                case "statusinfo":
                    return new StatusInfoHandler(_client, new StatusInfoRequest((string)rawRequest));
                #endregion
                default:
                    return null;
            }
        }
    }
}