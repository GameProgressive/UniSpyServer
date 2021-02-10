using PresenceConnectionManager.Abstraction.BaseClass;
using PresenceConnectionManager.Entity.Enumerate;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceConnectionManager.Handler.CmdHandler.General;
using PresenceConnectionManager.Network;
using PresenceConnectionManager.Structure;
using PresenceSearchPlayer.Entity.Enumerate;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Database.DatabaseModel.MySql;
using UniSpyLib.Logging;

namespace PresenceConnectionManager.Handler.CmdHandler
{
    internal class LoginHandler : PCMCmdHandlerBase
    {
        protected new LoginRequest _request => (LoginRequest)base._request;
        protected new LoginResult _result
        {
            get => (LoginResult)base._result;
            set => base._result = value;
        }
        public LoginHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new LoginResult();
        }

        protected override void RequestCheck()
        {
            switch (_request.LoginType)
            {
                case LoginType.NickEmail:
                    _session.UserInfo.Nick = _request.Nick;
                    _session.UserInfo.Email = _request.Email;
                    _session.UserInfo.NamespaceID = _request.NamespaceID;
                    break;
                case LoginType.UniquenickNamespaceID:
                    _session.UserInfo.UniqueNick = _request.Uniquenick;
                    _session.UserInfo.NamespaceID = _request.NamespaceID;
                    break;
                case LoginType.AuthToken:
                    _session.UserInfo.AuthToken = _request.AuthToken;
                    break;
                default:
                    LogWriter.ToLog(LogEventLevel.Error, "Unknown login method detected!");
                    break;
            }
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
            catch
            {
                _result.ErrorCode = GPErrorCode.DatabaseError;
            }

            //Arves is correct
            if (_result.ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (!IsChallengeCorrect())
            {
                _result.ErrorCode = GPErrorCode.LoginBadPassword;
                return;
            }

            if (!_result.DatabaseResults.EmailVerifiedFlag)
            {
                _result.ErrorCode = GPErrorCode.LoginBadProfile;
                return;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status.
            if (_result.DatabaseResults.BannedFlag)
            {
                _result.ErrorCode = GPErrorCode.LoginProfileDeleted;
                return;
            }

            ChallengeProofData proofData = new ChallengeProofData(
                            _request.UserData,
                            _request.LoginType,
                            _request.PartnerID,
                            ChallengeProofData.ServerChallenge,
                            _request.UserChallenge,
                            _result.DatabaseResults.PasswordHash);

            _result.ResponseProof =
               ChallengeProof.GenerateProof(proofData);
        }

        protected override void ResponseConstruct()
        {
            _response = new LoginResponse(_request, _result);
        }
        protected override void Response()
        {
            base.Response();
            //Arves is correct we need to check this
            if (_result.ErrorCode != GPErrorCode.NoError)
            {
                return;
            }

            if (_result == null)
            {
                return;
            }

            _session.UserInfo.UserStatus = GPStatus.Online;
            _session.UserInfo.UserID = _result.DatabaseResults.UserID;
            _session.UserInfo.ProfileID = _result.DatabaseResults.ProfileID;
            _session.UserInfo.SubProfileID = _result.DatabaseResults.SubProfileID;
            //_session.UserData.ProductID =
            _session.UserInfo.GameName = _request.GameName;
            _session.UserInfo.GamePort = _request.GamePort;
            _session.UserInfo.LoginStatus = LoginStatus.Completed;
            _session.UserInfo.SDKRevision = _request.SDKType;

            PCMServer.LoggedInSession.GetOrAdd(_session.Id, _session);
            new SDKRevisionHandler(_session, _request).Handle();
        }

        private void NickEmailLogin()
        {
            //Check email existence
            using (var db = new retrospyContext())
            {
                var email = from u in db.Users
                            where u.Email == _session.UserInfo.Email
                            select u.Userid;

                if (email.Count() == 0)
                {
                    _result.ErrorCode = GPErrorCode.LoginBadEmail;
                    return;
                }

                //Grab information from database
                // default namespace id = 0
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where u.Email == _session.UserInfo.Email
                           && p.Nick == _session.UserInfo.Nick
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
                    _result.ErrorCode = GPErrorCode.LoginBadProfile;
                    return;
                }
                _result.DatabaseResults = info.First();
            }
        }

        private void UniquenickLogin()
        {
            using (var db = new retrospyContext())
            {
                var info = from n in db.Subprofiles
                           join p in db.Profiles on n.Profileid equals p.Profileid
                           join u in db.Users on p.Userid equals u.Userid
                           where n.Uniquenick == _session.UserInfo.UniqueNick
                           && n.Namespaceid == _session.UserInfo.NamespaceID
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
                    _result.ErrorCode = GPErrorCode.LoginBadUniquenick;
                    return;
                }
                _result.DatabaseResults = info.First();
            }
        }

        private void AuthtokenLogin()
        {
            using (var db = new retrospyContext())
            {
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where n.Authtoken == _session.UserInfo.AuthToken
                           && n.Partnerid == _session.UserInfo.PartnerID
                           && n.Namespaceid == _session.UserInfo.NamespaceID
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
                    _result.ErrorCode = GPErrorCode.LoginBadPreAuth;
                    return;
                }
                _result.DatabaseResults = info.First();
            }
        }

        protected bool IsChallengeCorrect()
        {
            ChallengeProofData proofData = new ChallengeProofData(
                _request.UserData,
                _request.LoginType,
                _request.PartnerID,
               _request.UserChallenge,
               ChallengeProofData.ServerChallenge,
               _result.DatabaseResults.PasswordHash);

            string response = ChallengeProof.GenerateProof(proofData);

            if (_request.Response == response)
            {
                return true;
            }
            return false;
        }
    }
}
