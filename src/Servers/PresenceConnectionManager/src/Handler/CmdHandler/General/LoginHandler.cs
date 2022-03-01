using System.Linq;
using UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Contract;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Enumerate;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Request;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Response;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure.Result;
using UniSpyServer.Servers.PresenceConnectionManager.Structure;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.Login;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.UniSpyLib.Database.DatabaseModel;

namespace UniSpyServer.Servers.PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("login")]
    public sealed class LoginHandler : CmdHandlerBase
    {
        private new LoginRequest _request => (LoginRequest)base._request;
        private new LoginResult _result { get => (LoginResult)base._result; set => base._result = value; }
        public LoginHandler(IClient client, IRequest request) : base(client, request)
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

            LoginChallengeProof proofData = new LoginChallengeProof(
                _request.UserData,
                (LoginType)_request.Type,
                _request.PartnerID,
                LoginChallengeProof.ServerChallenge,
                _request.UserChallenge,
                _result.DatabaseResults.PasswordHash);

            _result.ResponseProof =
                LoginChallengeProof.GenerateProof(proofData);

            _client.Info.LoginPhase = LoginStatus.Completed;
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
            using (var db = new UniSpyContext())
            {
                var email = from u in db.Users
                            where u.Email == _request.Email
                            select u.UserId;

                if (email.Count() == 0)
                {
                    throw new GPLoginBadEmailException($"email: {_request.Email} is invalid.");
                }

                // Grab user from database via email and nick
                // Default namespaceID is 0
                var info = from u in db.Users
                           join p in db.Profiles on u.UserId equals p.Userid
                           join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                           where u.Email == _request.Email
                           && p.Nick == _request.Nick
                           select new { u, p, n };

                if (info.Count() != 1)
                {
                    throw new GPLoginBadProfileException();
                }
                _client.Info.UserInfo = info.First().u;
                _client.Info.ProfileInfo = info.First().p;
                _client.Info.SubProfileInfo = info.First().n;
            }
        }

        private void UniquenickLogin()
        {
            using (var db = new UniSpyContext())
            {
                var info = from n in db.Subprofiles
                           join p in db.Profiles on n.ProfileId equals p.ProfileId
                           join u in db.Users on p.Userid equals u.UserId
                           where n.Uniquenick == _request.UniqueNick
                           && n.NamespaceId == _request.NamespaceID
                           select new { u, p, n };
                if (info.Count() != 1)
                {
                    throw new GPLoginBadUniquenickException($"The uniquenick: {_request.UniqueNick} is invalid.");
                }
                _client.Info.UserInfo = info.First().u;
                _client.Info.ProfileInfo = info.First().p;
                _client.Info.SubProfileInfo = info.First().n;
            }
        }

        private void AuthtokenLogin()
        {
            using (var db = new UniSpyContext())
            {
                var info = from u in db.Users
                           join p in db.Profiles on u.UserId equals p.Userid
                           join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                           where n.Authtoken == _request.AuthToken
                           && n.PartnerId == _request.PartnerID
                           && n.NamespaceId == _request.NamespaceID
                           select new { u, p, n };

                if (info.Count() != 1)
                {
                    throw new GPLoginBadPreAuthException("The pre-authentication token is invalid.");
                }
                _client.Info.UserInfo = info.First().u;
                _client.Info.ProfileInfo = info.First().p;
                _client.Info.SubProfileInfo = info.First().n;
            }
        }

        private void IsChallengeCorrect()
        {
            LoginChallengeProof proofData = new LoginChallengeProof(
                _request.UserData,
                (LoginType)_request.Type,
                _request.PartnerID,
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
