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
        public LoginHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
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
            }
            catch (System.Exception e)
            {
                throw new GPDatabaseException(e.Message);
            }


            if (!IsChallengeCorrect())
            {
                throw new GPLoginBadPasswordException();
            }

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
        }

        protected override void ResponseConstruct()
        {
            _response = new LoginResponse(_request, _result);
        }

        protected override void Response()
        {
            base.Response();
            //Arves is correct we need to check this
            switch (_request.Type)
            {
                case LoginType.NickEmail:
                    _session.UserInfo.BasicInfo.Nick = _request.Nick;
                    _session.UserInfo.BasicInfo.Email = _request.Email;
                    _session.UserInfo.BasicInfo.UniqueNick = _result.DatabaseResults.UniqueNick;
                    break;
                case LoginType.UniquenickNamespaceID:
                    _session.UserInfo.BasicInfo.UniqueNick = _request.UniqueNick;
                    _session.UserInfo.BasicInfo.NamespaceId = _request.NamespaceID;
                    _session.UserInfo.BasicInfo.ProductId = _request.ProductID;
                    _session.UserInfo.SDKRevision.SDKRevisionType = _request.SDKRevisionType;
                    break;
                case LoginType.AuthToken:
                    _session.UserInfo.BasicInfo.AuthToken = _request.AuthToken;
                    _session.UserInfo.BasicInfo.ProductId = _request.ProductID;
                    break;
                default:
                    throw new GPParseException("Invalid login method detected.");
            }

            _session.UserInfo.Status.CurrentStatus = GPStatusCode.Online;
            _session.UserInfo.BasicInfo.UserId = _result.DatabaseResults.UserID;
            _session.UserInfo.BasicInfo.ProfileId = _result.DatabaseResults.ProfileId;
            _session.UserInfo.BasicInfo.SubProfileId = _result.DatabaseResults.SubProfileID;
            _session.UserInfo.BasicInfo.GameName = _request.GameName;
            _session.UserInfo.BasicInfo.GamePort = _request.GamePort;
            _session.UserInfo.BasicInfo.LoginStatus = LoginStatus.Completed;

            new SdkRevisionHandler(_session, _request).Handle();
        }

        private void NickEmailLogin()
        {
            //Check email existence
            using (var db = new UniSpyContext())
            {
                var email = from u in db.Users
                            where u.Email == _request.Email
                            select u.Userid;

                if (email.Count() == 0)
                {
                    throw new GPLoginBadEmailException($"email: {_request.Email} is invalid.");
                }

                // Grab user from database via email and nick
                // Default namespaceID is 0
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                           where u.Email == _request.Email
                           && p.Nick == _request.Nick
                           select new LogInDataModel
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileId = p.ProfileId,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = (bool)u.Banned
                           };

                if (info.Count() != 1)
                {
                    throw new GPLoginBadProfileException();
                }
                _result.DatabaseResults = info.First();
            }
        }

        private void UniquenickLogin()
        {
            using (var db = new UniSpyContext())
            {
                var info = from n in db.Subprofiles
                           join p in db.Profiles on n.ProfileId equals p.ProfileId
                           join u in db.Users on p.Userid equals u.Userid
                           where n.Uniquenick == _request.UniqueNick
                           && n.Namespaceid == _request.NamespaceID
                           select new LogInDataModel
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileId = p.ProfileId,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = (bool)u.Banned
                           };

                if (info.Count() != 1)
                {
                    throw new GPLoginBadUniquenickException($"The uniquenick: {_request.UniqueNick} is invalid.");
                }
                _result.DatabaseResults = info.First();
            }
        }

        private void AuthtokenLogin()
        {
            using (var db = new UniSpyContext())
            {
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.ProfileId equals n.ProfileId
                           where n.Authtoken == _request.AuthToken
                           && n.Partnerid == _request.PartnerID
                           && n.Namespaceid == _request.NamespaceID
                           select new LogInDataModel
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileId = p.ProfileId,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = (bool)u.Banned
                           };

                if (info.Count() != 1)
                {
                    throw new GPLoginBadPreAuthException("The pre-authentication token is invalid.");
                }
                _result.DatabaseResults = info.First();
            }
        }

        private bool IsChallengeCorrect()
        {
            LoginChallengeProof proofData = new LoginChallengeProof(
                _request.UserData,
                (LoginType)_request.Type,
                _request.PartnerID,
                _request.UserChallenge,
                LoginChallengeProof.ServerChallenge,
                _result.DatabaseResults.PasswordHash);

            string response = LoginChallengeProof.GenerateProof(proofData);

            if (_request.Response == response)
            {
                return true;
            }
            return false;
        }
    }
}
