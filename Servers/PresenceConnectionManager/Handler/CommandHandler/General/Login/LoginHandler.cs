using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.Login.Misc;
using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using System.Collections.Generic;
//using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using System.Linq;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{

    public class LoginHandler : GPCMHandlerBase
    {
        private Crc16 _crc = new Crc16(Crc16Mode.Standard);

        public LoginHandler(Dictionary<string, string> recv) : base(recv)
        {
        }

        protected override void CheckRequest(GPCMSession session)
        {
            // for pass operation id to session,Playerinfo
            base.CheckRequest(session);

            // Make sure we have all the required data to process this login
            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                _errorCode = GPErrorCode.Parse;
                return;
            }

            ParseDataBasedOnLoginType(session);

            ParseOtherData(session);
        }
        /// <summary>
        /// Parse everything into PlayerInfo, so we can use it later.
        /// </summary>
        /// <param name="session"></param>
        private void ParseDataBasedOnLoginType(GPCMSession session)
        {
            session.UserInfo.UserChallenge = _recv["challenge"];

            if (_recv.ContainsKey("uniquenick"))
            {
                session.UserInfo.LoginType = LoginType.Uniquenick;
                session.UserInfo.UniqueNick = _recv["uniquenick"];
                session.UserInfo.UserData = _recv["uniquenick"];
                return;
            }
            if (_recv.ContainsKey("authtoken"))
            {
                session.UserInfo.LoginType = LoginType.AuthToken;
                session.UserInfo.AuthToken = _recv["authtoken"];
                session.UserInfo.UserData = _recv["authtoken"];
                return;
            }
            if (_recv.ContainsKey("user"))
            {
                session.UserInfo.LoginType = LoginType.Nick;
                session.UserInfo.UserData = _recv["user"];
                string user = _recv["user"];

                int Pos = user.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= user.Length)
                {
                    _errorCode = GPErrorCode.LoginBadEmail;
                    return;
                }
                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);

                session.UserInfo.Nick = nick;
                session.UserInfo.Email = email;
                session.UserInfo.LoginType = LoginType.Nick;
                return;
            }

            //if no login method found we can not continue.
            session.ToLog("Unknown login method detected!");
            _errorCode = GPErrorCode.Parse;

        }

        private void ParseOtherData(GPCMSession session)
        {
            if (_recv.ContainsKey("partnerid"))
            {
                if (!uint.TryParse(_recv["partnerid"], out session.UserInfo.PartnerID))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
            if (_recv.ContainsKey("namespaceid"))
            {
                if (!uint.TryParse(_recv["namespaceid"], out session.UserInfo.NamespaceID))
                    _errorCode = GPErrorCode.Parse;
                // the default namespaceid = 0
            }
            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                if (!uint.TryParse(_recv["sdkrevision"], out session.UserInfo.SDKRevision))
                    _errorCode = GPErrorCode.Parse;
            }
            if (_recv.ContainsKey("gamename"))
            {
                session.UserInfo.GameName = _recv["gamename"];
            }
            if (_recv.ContainsKey("port"))
                if (!uint.TryParse(_recv["port"], out session.UserInfo.PeerPort))
                    _errorCode = GPErrorCode.Parse;

            if (_recv.ContainsKey("quiet"))
                if (!uint.TryParse(_recv["quiet"], out session.UserInfo.QuietModeFlag))
                    _errorCode = GPErrorCode.Parse;
        }

        protected override void ConstructResponse(GPCMSession session)
        {
            session.UserInfo.SessionKey
                = _crc.ComputeChecksum(session.UserInfo.Nick + session.UserInfo.NamespaceID);

            string responseProof = ChallengeProof.GenerateProof
            (
                session.UserInfo.UserData,
                session.UserInfo.LoginType,
                session.UserInfo.PartnerID,
                session.UserInfo.ServerChallenge,
                session.UserInfo.UserChallenge,
                session.UserInfo.PasswordHash
            );

            _sendingBuffer = @"\lc\2\sesskey\" + session.UserInfo.SessionKey;
            _sendingBuffer += @"\proof\" + responseProof;
            _sendingBuffer += @"\userid\" + session.UserInfo.Userid;
            _sendingBuffer += @"\profileid\" + session.UserInfo.Profileid;

            if (session.UserInfo.LoginType != LoginType.Nick)
                _sendingBuffer += @"\uniquenick\" + session.UserInfo.UniqueNick;

            _sendingBuffer += @"\lt\" + session.Id.ToString().Replace("-", "").Substring(0, 22) + "__";
            _sendingBuffer += @"\id\" + _operationID + @"\final\";

            session.UserInfo.LoginProcess = LoginStatus.Completed;
        }

        protected override void DataBaseOperation(GPCMSession session)
        {

            switch (session.UserInfo.LoginType)
            {
                case LoginType.Nick:
                    NickEmailLogin(session);
                    break;
                case LoginType.Uniquenick:
                    UniquenickLogin(session);
                    break;
                case LoginType.AuthToken:
                    AuthtokenLogin(session);
                    break;
            }
            //check if errorcode equals database error we stop
            if (_errorCode == GPErrorCode.DatabaseError)
            {
                return;
            }

            if (!session.UserInfo.IsEmailVerified)
            {
                _errorCode = GPErrorCode.LoginBadEmail;
                return;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (session.UserInfo.IsBlocked)
            {
                _errorCode = GPErrorCode.LoginProfileDeleted;
                return;
            }

            if (!IsChallengeCorrect(session))
            {
                _errorCode = GPErrorCode.LoginBadPassword;
                return;
            }

        }


        private void NickEmailLogin(GPCMSession session)
        {
            //Check email existence
            using (var db = new RetrospyDB())
            {
                var email = from u in db.Users
                            where u.Email == session.UserInfo.Email
                            select u.Userid;

                if (email.Count() == 0)
                {
                    _errorCode = GPErrorCode.LoginBadEmail;
                }

                //Grab information from database
                var info = from u in db.Users
                           from p in db.Profiles
                           from n in db.Subprofiles
                           where u.Email == session.UserInfo.Email && p.Nick == session.UserInfo.Nick && n.Namespaceid == session.UserInfo.NamespaceID
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
                    _errorCode = GPErrorCode.LoginBadPassword;
                    return;
                }
                else if (info.Count() == 1)
                {
                    session.UserInfo.Userid = info.First().userid;
                    session.UserInfo.Profileid = info.First().profileid;
                    session.UserInfo.UniqueNick = info.First().uniquenick;
                    session.UserInfo.PasswordHash = info.First().password;
                    session.UserInfo.IsEmailVerified = info.First().emailVerified;
                    session.UserInfo.IsBlocked = info.First().blocked;
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }

            }
        }

        private void UniquenickLogin(GPCMSession session)
        {
            using (var db = new RetrospyDB())
            {
                var info = from n in db.Subprofiles
                           join p in db.Profiles on n.Profileid equals p.Profileid
                           join u in db.Users on p.Userid equals u.Userid
                           where n.Uniquenick == session.UserInfo.UniqueNick
                           && n.Namespaceid == session.UserInfo.NamespaceID
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
                    session.UserInfo.Userid = info.First().userid;
                    session.UserInfo.Profileid = info.First().profileid;
                    session.UserInfo.UniqueNick = info.First().uniquenick;
                    session.UserInfo.PasswordHash = info.First().password;
                    session.UserInfo.IsEmailVerified = info.First().emailVerified;
                    session.UserInfo.IsBlocked = info.First().blocked;
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }
            }

        }

        private void AuthtokenLogin(GPCMSession session)
        {

            using (var db = new RetrospyDB())
            {
                var info = from u in db.Users
                           join p in db.Profiles on u.Userid equals p.Userid
                           join n in db.Subprofiles on p.Profileid equals n.Profileid
                           where n.Authtoken == session.UserInfo.AuthToken && n.Partnerid == session.UserInfo.PartnerID
                           && n.Namespaceid == session.UserInfo.NamespaceID
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
                if (info.Count() == 0)
                {
                    _errorCode = GPErrorCode.LoginBadUniquenick;
                    return;
                }
                else if (info.Count() == 1)
                {
                    session.UserInfo.Userid = info.First().userid;
                    session.UserInfo.Profileid = info.First().profileid;
                    session.UserInfo.UniqueNick = info.First().uniquenick;
                    session.UserInfo.PasswordHash = info.First().password;
                    session.UserInfo.IsEmailVerified = info.First().emailVerified;
                    session.UserInfo.IsBlocked = info.First().blocked;
                    session.UserInfo.Nick = info.First().nick;
                    session.UserInfo.Email = info.First().email;
                }
                else
                {
                    _errorCode = GPErrorCode.DatabaseError;
                }

            }
        }

        protected override void Response(GPCMSession session)
        {
            base.Response(session);
            session.UserInfo.StatusCode = GPStatus.Online;
            GPCMServer.LoggedInSession.TryAdd(session.Id, session);
            SDKRevision.ExtendedFunction(session);
        }
        protected bool IsChallengeCorrect(GPCMSession session)
        {
            string response = ChallengeProof.GenerateProof
                (
                session.UserInfo.UserData,
                session.UserInfo.LoginType,
                session.UserInfo.PartnerID,
                session.UserInfo.UserChallenge,
                session.UserInfo.ServerChallenge,
                session.UserInfo.PasswordHash
                );
            if (_recv["response"] == response)
            {
                return true;
            }
            return false;
        }
    }
}
