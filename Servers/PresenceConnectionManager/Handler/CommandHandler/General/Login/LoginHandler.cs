using GameSpyLib.Common.Entity.Interface;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
//using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using PresenceConnectionManager.Handler.Error;
using PresenceConnectionManager.Handler.General.Login.Misc;
using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using System.Collections.Generic;
using System.Linq;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{
    public class LoginHandler :  PCMCommandHandlerBase
    {
        private Crc16 _crc;

        public LoginHandler(IClient client, Dictionary<string, string> recv) : base(client, recv)
        {
            _crc = new Crc16(Crc16Mode.Standard);
        }

        protected override void CheckRequest()
        {
            base.CheckRequest();
            // Make sure we have all the required data to process this login
            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            ParseDataBasedOnLoginType();

            ParseOtherData();
        }

        public override void Handle()
        {
            CheckRequest();
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPCMError(_session, _errorCode, _operationID);
                return;
            }

            DataOperation();
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPCMError(_session, _errorCode, _operationID);
                return;
            }

            ConstructResponse();
            if (_errorCode == GPErrorCode.ConstructResponseError)
            {
                ErrorMsg.SendGPCMError(_session, _errorCode, _operationID);
                return;
            }

            Response();
        }

        /// <summary>
        /// Parse everything into PlayerInfo, so we can use it later.
        /// </summary>
        /// <param name="_session"></param>
        private void ParseDataBasedOnLoginType()
        {
            _session.UserInfo.UserChallenge = _recv["challenge"];

            if (_recv.ContainsKey("uniquenick"))
            {
                _session.UserInfo.LoginType = LoginType.Uniquenick;
                _session.UserInfo.UniqueNick = _recv["uniquenick"];
                _session.UserInfo.UserData = _recv["uniquenick"];
                return;
            }

            if (_recv.ContainsKey("authtoken"))
            {
                _session.UserInfo.LoginType = LoginType.AuthToken;
                _session.UserInfo.AuthToken = _recv["authtoken"];
                _session.UserInfo.UserData = _recv["authtoken"];
                return;
            }

            if (_recv.ContainsKey("user"))
            {
                _session.UserInfo.LoginType = LoginType.Nick;
                _session.UserInfo.UserData = _recv["user"];
                string user = _recv["user"];

                int Pos = user.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= user.Length)
                {
                    _errorCode = GPErrorCode.LoginBadEmail;
                    return;
                }
                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);

                _session.UserInfo.Nick = nick;
                _session.UserInfo.Email = email;
                _session.UserInfo.LoginType = LoginType.Nick;
                return;
            }

            //if no login method found we can not continue.
            LogWriter.ToLog(Serilog.Events.LogEventLevel.Error, "Unknown login method detected!");
            _errorCode = GPErrorCode.Parse;
        }

        private void ParseOtherData()
        {
            if (_recv.ContainsKey("partnerid"))
            {
                if (!uint.TryParse(_recv["partnerid"], out _session.UserInfo.PartnerID))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }

            if (_recv.ContainsKey("namespaceid"))
            {
                if (!uint.TryParse(_recv["namespaceid"], out _session.UserInfo.NamespaceID))
                {
                    // the default namespaceid = 0
                    _errorCode = GPErrorCode.Parse;
                }
            }

            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                if (!uint.TryParse(_recv["sdkrevision"], out _session.UserInfo.SDKRevision))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }

            if (_recv.ContainsKey("gamename"))
            {
                _session.UserInfo.GameName = _recv["gamename"];
            }

            if (_recv.ContainsKey("port"))
            {
                if (!uint.TryParse(_recv["port"], out _session.UserInfo.PeerPort))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }

            if (_recv.ContainsKey("quiet"))
            {
                if (!uint.TryParse(_recv["quiet"], out _session.UserInfo.QuietModeFlag))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
        }

        protected override void ConstructResponse()
        {
            _session.UserInfo.SessionKey
                = _crc.ComputeChecksum(_session.UserInfo.Nick + _session.UserInfo.UniqueNick + _session.UserInfo.NamespaceID);

            string responseProof = ChallengeProof.GenerateProof
            (
                _session.UserInfo.UserData,
                _session.UserInfo.LoginType,
                _session.UserInfo.PartnerID,
                _session.UserInfo.ServerChallenge,
                _session.UserInfo.UserChallenge,
                _session.UserInfo.PasswordHash
            );

            _sendingBuffer = @"\lc\2\sesskey\" + _session.UserInfo.SessionKey;
            _sendingBuffer += @"\proof\" + responseProof;
            _sendingBuffer += @"\userid\" + _session.UserInfo.Userid;
            _sendingBuffer += @"\profileid\" + _session.UserInfo.Profileid;

            if (_session.UserInfo.LoginType != LoginType.Nick)
            {
                _sendingBuffer += @"\uniquenick\" + _session.UserInfo.UniqueNick;
            }

            _sendingBuffer += @"\lt\" + _session.Id.ToString().Replace("-", "").Substring(0, 22) + "__";
            _sendingBuffer += @"\id\" + _operationID + @"\final\";

            _session.UserInfo.LoginProcess = LoginStatus.Completed;
        }

        protected override void DataOperation()
        {
            switch (_session.UserInfo.LoginType)
            {
                case LoginType.Nick:
                    NickEmailLogin();
                    break;

                case LoginType.Uniquenick:
                    UniquenickLogin();
                    break;

                case LoginType.AuthToken:
                    AuthtokenLogin();
                    break;
            }

            //check if errorcode equals database error we stop
            if (_errorCode == GPErrorCode.DatabaseError)
            {
                return;
            }

            if (!_session.UserInfo.IsEmailVerified)
            {
                _errorCode = GPErrorCode.LoginBadEmail;
                return;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (_session.UserInfo.IsBlocked)
            {
                _errorCode = GPErrorCode.LoginProfileDeleted;
                return;
            }

            if (!IsChallengeCorrect())
            {
                _errorCode = GPErrorCode.LoginBadPassword;
                return;
            }
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
                           && n.Namespaceid == _session.UserInfo.NamespaceID
                           select new
                           {
                               userid = u.Userid,
                               profileid = p.Profileid,
                               uniquenick = n.Uniquenick,
                               password = u.Password,
                               emailVerified = u.Emailverified,
                               blocked = u.Banned
                           };

                if (info.Count() == 1)
                {
                    _session.UserInfo.Userid = info.First().userid;
                    _session.UserInfo.Profileid = info.First().profileid;
                    _session.UserInfo.UniqueNick = info.First().uniquenick;
                    _session.UserInfo.PasswordHash = info.First().password;
                    _session.UserInfo.IsEmailVerified = (bool)info.First().emailVerified;
                    _session.UserInfo.IsBlocked = info.First().blocked;
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }
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
                           select new
                           {
                               userid = u.Userid,
                               profileid = p.Profileid,
                               uniquenick = n.Uniquenick,
                               password = u.Password,
                               emailVerified = u.Emailverified,
                               blocked = u.Banned
                           };

                if (info.Count() == 0)
                {
                    _errorCode = GPErrorCode.LoginBadUniquenick;
                    return;
                }
                else if (info.Count() == 1)
                {
                    _session.UserInfo.Userid = info.First().userid;
                    _session.UserInfo.Profileid = info.First().profileid;
                    _session.UserInfo.UniqueNick = info.First().uniquenick;
                    _session.UserInfo.PasswordHash = info.First().password;
                    _session.UserInfo.IsEmailVerified = (bool)info.First().emailVerified;
                    _session.UserInfo.IsBlocked = info.First().blocked;
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }
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
                           select new
                           {
                               profileid = n.Profileid,
                               nick = p.Nick,
                               uniquenick = n.Uniquenick,
                               userid = u.Userid,
                               email = u.Email,
                               password = u.Password,
                               emailVerified = u.Emailverified,
                               blocked = u.Banned
                           };

                if (info.Count() == 1)
                {
                    _session.UserInfo.Userid = info.First().userid;
                    _session.UserInfo.Profileid = info.First().profileid;
                    _session.UserInfo.UniqueNick = info.First().uniquenick;
                    _session.UserInfo.PasswordHash = info.First().password;
                    _session.UserInfo.IsEmailVerified = (bool)info.First().emailVerified;
                    _session.UserInfo.IsBlocked = info.First().blocked;
                    _session.UserInfo.Nick = info.First().nick;
                    _session.UserInfo.Email = info.First().email;
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }
            }
        }

        protected override void Response()
        {
            base.Response();
            _session.UserInfo.StatusCode = GPStatus.Online;
            GPCMServer.LoggedInSession.GetOrAdd(_session.Id, _session);
            SDKRevision.ExtendedFunction(_session);
        }

        protected bool IsChallengeCorrect()
        {
            string response = ChallengeProof.GenerateProof
            (
                _session.UserInfo.UserData,
                _session.UserInfo.LoginType,
                _session.UserInfo.PartnerID,
                _session.UserInfo.UserChallenge,
                _session.UserInfo.ServerChallenge,
                _session.UserInfo.PasswordHash
            );

            if (_recv["response"] == response)
            {
                return true;
            }
            return false;
        }
    }
}
