using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Contract;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceConnectionManager.Structure;
using PresenceSearchPlayer.Entity.Exception.General;
using PresenceSearchPlayer.Entity.Exception.Login;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    [HandlerContract("login")]
    internal sealed class LoginHandler : CmdHandlerBase
    {
        private new LoginRequest _request => (LoginRequest)base._request;
        private new LoginResult _result
        {
            get => (LoginResult)base._result;
            set => base._result = value;
        }
        public LoginHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new LoginResult();
        }

        protected override void DataOperation()
        {
            try
            {
                switch (_request.LoginType)
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

            // Check the status of the account.
            // If the single profile is banned, the account or the player status.
            if (_result.DatabaseResults.BannedFlag)
            {
                throw new GPLoginProfileDeletedException();
            }

            LoginChallengeProof proofData = new LoginChallengeProof(
                            _request.UserData,
                            _request.LoginType,
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
            switch (_request.LoginType)
            {
                case LoginType.NickEmail:
                    _session.UserInfo.BasicInfo.Nick = _request.Nick;
                    _session.UserInfo.BasicInfo.Email = _request.Email;
                    _session.UserInfo.BasicInfo.NamespaceID = _request.NamespaceID;
                    _session.UserInfo.BasicInfo.UniqueNick = _result.DatabaseResults.UniqueNick;
                    break;
                case LoginType.UniquenickNamespaceID:
                    _session.UserInfo.BasicInfo.UniqueNick = _request.UniqueNick;
                    _session.UserInfo.BasicInfo.NamespaceID = _request.NamespaceID;
                    break;
                case LoginType.AuthToken:
                    _session.UserInfo.BasicInfo.AuthToken = _request.AuthToken;
                    break;
                default:
                    throw new GPParseException("Invalid login method detected.");
            }

            _session.UserInfo.Status.CurrentStatus = GPStatusCode.Online;
            _session.UserInfo.BasicInfo.UserID = _result.DatabaseResults.UserID;
            _session.UserInfo.BasicInfo.ProfileID = _result.DatabaseResults.ProfileID;
            _session.UserInfo.BasicInfo.SubProfileID = _result.DatabaseResults.SubProfileID;
            _session.UserInfo.BasicInfo.ProductID = _request.ProductID;
            _session.UserInfo.BasicInfo.GameName = _request.GameName;
            _session.UserInfo.BasicInfo.GamePort = _request.GamePort;
            _session.UserInfo.BasicInfo.LoginStatus = LoginStatus.Completed;
            _session.UserInfo.SDKRevision.SDKRevisionType = _request.SDKRevisionType;

            new SDKRevisionHandler(_session, _request).Handle();
        }

        private void NickEmailLogin()
        {
            //Check email existence
            using (var db = new unispyContext())
            {
                var email = from u in db.Users
                            where u.Email == _request.Email
                            select u.Userid;

                if (email.Count() == 0)
                {
                    throw new GPLoginBadEmailException($"email: {_request.Email} is invalid.");
                }

                //Grab information from database
                // default namespace id = 0
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where u.Email == _request.Email
                           && p.Nick == _request.Nick
                           && n.Namespaceid == _request.NamespaceID
                           select new LogInDataModel
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileID = p.Profileid,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
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
            using (var db = new unispyContext())
            {
                var info = from n in db.Subprofiles
                           join p in db.Profiles on n.Profileid equals p.Profileid
                           join u in db.Users on p.Userid equals u.Userid
                           where n.Uniquenick == _request.UniqueNick
                           && n.Namespaceid == _request.NamespaceID
                           select new LogInDataModel
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileID = p.Profileid,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
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
            using (var db = new unispyContext())
            {
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where n.Authtoken == _request.AuthToken
                           && n.Partnerid == _request.PartnerID
                           && n.Namespaceid == _request.NamespaceID
                           select new LogInDataModel
                           {
                               Email = u.Email,
                               UserID = u.Userid,
                               ProfileID = p.Profileid,
                               SubProfileID = n.Subprofileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
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
                _request.LoginType,
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
