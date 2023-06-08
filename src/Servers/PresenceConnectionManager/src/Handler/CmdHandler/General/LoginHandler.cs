using UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpy.Server.PresenceConnectionManager.Enumerate;
using UniSpy.Server.PresenceConnectionManager.Contract.Request;
using UniSpy.Server.PresenceConnectionManager.Contract.Response;
using UniSpy.Server.PresenceConnectionManager.Contract.Result;
using UniSpy.Server.PresenceConnectionManager.Structure;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.PresenceSearchPlayer.Exception.Login;
using UniSpy.Server.PresenceConnectionManager.Application;

namespace UniSpy.Server.PresenceConnectionManager.Handler.CmdHandler.General
{

    public sealed class LoginHandler : CmdHandlerBase
    {
        private new LoginRequest _request => (LoginRequest)base._request;
        private new LoginResult _result { get => (LoginResult)base._result; set => base._result = value; }
        public LoginHandler(Client client, LoginRequest request) : base(client, request)
        {
            _result = new LoginResult();
        }

        protected override void DataOperation()
        {
            try
            {
                switch (_request.Type)
                {
                    case LoginType.NickEmail:
                        NickEmailLogin();
                        break;

                    case LoginType.UniquenickNamespaceID:
                        UniquenickLogin();
                        break;

                    case LoginType.AuthToken:
                        AuthtokenLogin();
                        break;
                        // loginticket
                }
                _result.DatabaseResults = new LogInDataModel
                {
                    Email = _client.Info.UserInfo.Email,
                    UserId = _client.Info.UserInfo.UserId,
                    ProfileId = _client.Info.ProfileInfo.ProfileId,
                    SubProfileId = _client.Info.SubProfileInfo.SubProfileId,
                    Nick = _client.Info.ProfileInfo.Nick,
                    UniqueNick = _client.Info.SubProfileInfo.Uniquenick,
                    PasswordHash = _client.Info.UserInfo.Password,
                    EmailVerifiedFlag = (bool)_client.Info.UserInfo.Emailverified,
                    BannedFlag = (bool)_client.Info.UserInfo.Banned,
                    NamespaceId = _client.Info.SubProfileInfo.NamespaceId
                };
                _client.Info.Status.CurrentStatus = GPStatusCode.Online;
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException(e.Message);
            }

            IsChallengeCorrect();

            if (!_result.DatabaseResults.EmailVerifiedFlag)
            {
                throw new GPLoginBadProfileException();
            }

            // Check if the user is flagged as banned.
            if (_result.DatabaseResults.BannedFlag)
            {
                throw new GPLoginProfileDeletedException();
            }

            // PartnerID is optional
            LoginChallengeProof proofData = new LoginChallengeProof(
                _request.UserData,
                (LoginType)_request.Type,
                _request.PartnerID,
                LoginChallengeProof.ServerChallenge,
                _request.UserChallenge,
                _result.DatabaseResults.PasswordHash);

            _result.ResponseProof =
                LoginChallengeProof.GenerateProof(proofData);

            _client.Info.LoginStat = LoginStatus.Completed;
        }

        protected override void ResponseConstruct()
        {
            _response = new LoginResponse(_request, _result);
        }

        protected override void Response()
        {
            base.Response();
            //Arves is correct we need to check this
            // save information to client object
            new SdkRevisionHandler(_client, _request).Handle();
        }

        private void NickEmailLogin()
        {
            //Check email existence
            if (!StorageOperation.Persistance.IsEmailExist(_request.Email))
            {
                throw new GPLoginBadEmailException($"email: {_request.Email} is invalid.");
            }

            (_client.Info.UserInfo, _client.Info.ProfileInfo, _client.Info.SubProfileInfo) = PresenceConnectionManager.Application.StorageOperation.Persistance.GetUsersInfos(_request.Email, _request.Nick);
        }


        private void UniquenickLogin()
        {
            (_client.Info.UserInfo, _client.Info.ProfileInfo, _client.Info.SubProfileInfo) = PresenceConnectionManager.Application.StorageOperation.Persistance.GetUsersInfos(_request.UniqueNick, (int)_request.NamespaceID);
        }

        private void AuthtokenLogin()
        {
            (_client.Info.UserInfo, _client.Info.ProfileInfo, _client.Info.SubProfileInfo) = PresenceConnectionManager.Application.StorageOperation.Persistance.GetUsersInfos(_request.AuthToken, (int)_request.PartnerID, (int)_request.NamespaceID);
        }

        private void IsChallengeCorrect()
        {
            // PartnerID is optional
            LoginChallengeProof proofData = new LoginChallengeProof(
                _request.UserData,
                (LoginType)_request.Type,
                (int?)_request.PartnerID,
                _request.UserChallenge,
                LoginChallengeProof.ServerChallenge,
                _result.DatabaseResults.PasswordHash);

            string response = LoginChallengeProof.GenerateProof(proofData);

            if (_request.Response != response)
            {
                throw new GPLoginBadPasswordException("The response is not valid, this maybe caused by wrong password.");
            }
        }
    }
}
