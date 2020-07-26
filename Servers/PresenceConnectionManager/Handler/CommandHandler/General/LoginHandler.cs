using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using GameSpyLib.Logging;
using PresenceConnectionManager.Entity.Enumerator;
using PresenceConnectionManager.Entity.Structure;
using PresenceConnectionManager.Entity.Structure.Request;
using PresenceConnectionManager.Structure;
//using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using PresenceSearchPlayer.Entity.Enumerator;
using Serilog.Events;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{
    internal class LoginDBResult
    {
        public uint Userid;
        public uint Profileid;
        public string Nick;
        public string Email;
        public string UniqueNick;
        public string PasswordHash;
        public bool EmailVerifiedFlag;
        public bool BannedFlag;
        public uint NamespaceID;
    }

    public class LoginHandler : PCMCommandHandlerBase
    {
        private Crc16 _crc;
        protected LoginRequest _request;
        private LoginDBResult _result;
        public LoginHandler(ISession session, Dictionary<string, string> recv) : base(session, recv)
        {
            _crc = new Crc16(Crc16Mode.Standard);
            _request = new LoginRequest(recv);
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();

            switch (_request.LoginType)
            {
                case LoginType.NickEmail:
                    _session.UserInfo.Nick = _request.Nick;
                    _session.UserInfo.Email = _request.Email;
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

            //check if errorcode equals database error we stop.
            if (_errorCode == GPErrorCode.DatabaseError)
            {
                return;
            }

            if (!IsChallengeCorrect())
            {
                _errorCode = GPErrorCode.LoginBadPassword;
                return;
            }

            if (!_result.EmailVerifiedFlag)
            {
                _errorCode = GPErrorCode.LoginBadEmail;
                return;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status.
            if (_result.BannedFlag)
            {
                _errorCode = GPErrorCode.LoginProfileDeleted;
                return;
            }

           
        }

        protected override void ConstructResponse()
        {
            if (_errorCode != GPErrorCode.NoError)
            {
                BuildErrorMessage();
                return;
            }

            string checkSumStr = _result.Nick
                + _result.UniqueNick
                + _result.NamespaceID;

            _session.UserInfo.SessionKey = _crc.ComputeChecksum(checkSumStr);

            ChallengeProofData proofData = new ChallengeProofData(
              _request.UserData,
              _request.LoginType,
              _request.PartnerID,
             _request.UserChallenge,
             _result.PasswordHash);

            string responseProof =
                ChallengeProof.GenerateProof(proofData);

            _sendingBuffer = @"\lc\2\sesskey\" + _session.UserInfo.SessionKey;
            _sendingBuffer += @"\proof\" + responseProof;
            _sendingBuffer += @"\userid\" + _session.UserInfo.UserID;
            _sendingBuffer += @"\profileid\" + _session.UserInfo.ProfileID;

            if (_request.LoginType != LoginType.NickEmail)
            {
                _sendingBuffer += @"\uniquenick\" + _session.UserInfo.UniqueNick;
            }
            _sendingBuffer += @$"\lt\{_session.UserInfo.LoginTicket}";
            _sendingBuffer += $@"\id\{_request.OperationID}\final\";

            _session.UserInfo.LoginStatus = LoginStatus.Completed;
        }

        protected override void Response()
        {
            base.Response();
            _session.UserInfo.StatusCode = GPStatus.Online;
            PCMServer.LoggedInSession.GetOrAdd(_session.Id, _session);
            SDKRevision.ExtendedFunction(_session);
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
                    _errorCode = GPErrorCode.LoginBadEmail;
                    return;
                }

                //Grab information from database
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where u.Email == _session.UserInfo.Email
                           && p.Nick == _session.UserInfo.Nick
                           select new LoginDBResult
                           {
                               Email = u.Email,
                               Userid = u.Userid,
                               Profileid = p.Profileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
                           };

                if (info.Count() != 1)
                {
                    _errorCode = GPErrorCode.DatabaseError;
                    return;

                }

                _result = info.First();
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
                           select new LoginDBResult
                           {
                               Email = u.Email,
                               Userid = u.Userid,
                               Profileid = p.Profileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
                           };

                if (info.Count() != 1)
                {
                    _errorCode = GPErrorCode.LoginBadUniquenick;
                    return;
                }
                _result = info.First();
            }
        }

        private void AuthtokenLogin()
        {
            using (var db = new retrospyContext())
            {
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where n.Authtoken == _session.UserInfo.AuthToken && n.Partnerid == _session.UserInfo.PartnerID
                           && n.Namespaceid == _session.UserInfo.NamespaceID
                           select new LoginDBResult
                           {
                               Email = u.Email,
                               Userid = u.Userid,
                               Profileid = p.Profileid,
                               Nick = p.Nick,
                               UniqueNick = n.Uniquenick,
                               PasswordHash = u.Password,
                               NamespaceID = n.Namespaceid,
                               EmailVerifiedFlag = (bool)u.Emailverified,
                               BannedFlag = u.Banned
                           };

                if (info.Count() != 1)
                {
                    _errorCode = GPErrorCode.DatabaseError;
                    return;
                }
                _result = info.First();
            }
        }

        protected bool IsChallengeCorrect()
        {
            ChallengeProofData proofData = new ChallengeProofData(
                _request.UserData,
                _request.LoginType,
                _request.PartnerID,
               _request.UserChallenge,
               _result.PasswordHash);

            string response = ChallengeProof.GenerateProof(proofData);

            if (_request.Response == response)
            {
                return true;
            }
            return false;
        }
    }
}
