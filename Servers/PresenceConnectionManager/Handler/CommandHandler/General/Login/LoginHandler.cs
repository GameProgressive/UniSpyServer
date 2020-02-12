using System;
using System.Collections.Generic;
using GameSpyLib.Database.DatabaseModel.MySql;
using GameSpyLib.Encryption;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.Login.Misc;
//using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using System.Linq;

namespace PresenceConnectionManager.Handler.General.Login.LoginMethod
{

    public class LoginHandler : GPCMHandlerBase
    {
        Crc16 _crc = new Crc16(Crc16Mode.Standard);

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
            session.PlayerInfo.UserChallenge = _recv["challenge"];

            if (_recv.ContainsKey("uniquenick"))
            {
                session.PlayerInfo.LoginType = LoginType.Uniquenick;
                session.PlayerInfo.UniqueNick = _recv["uniquenick"];
                session.PlayerInfo.UserData = _recv["uniquenick"];
                return;
            }
            if (_recv.ContainsKey("authtoken"))
            {
                session.PlayerInfo.LoginType = LoginType.AuthToken;
                session.PlayerInfo.AuthToken = _recv["authtoken"];
                session.PlayerInfo.UserData = _recv["authtoken"];
                return;
            }
            if (_recv.ContainsKey("user"))
            {
                session.PlayerInfo.LoginType = LoginType.Nick;
                session.PlayerInfo.UserData = _recv["user"];
                string user = _recv["user"];

                int Pos = user.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= user.Length)
                {
                    _errorCode = GPErrorCode.LoginBadEmail;
                    return;
                }
                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);

                session.PlayerInfo.Nick = nick;
                session.PlayerInfo.Email = email;
                session.PlayerInfo.LoginType = LoginType.Nick;
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
                if (!uint.TryParse(_recv["partnerid"], out session.PlayerInfo.PartnerID))
                {
                    _errorCode = GPErrorCode.Parse;
                }
            }
            if (_recv.ContainsKey("namespaceid"))
            {
                if (!uint.TryParse(_recv["namespaceid"], out session.PlayerInfo.NamespaceID))
                    _errorCode = GPErrorCode.Parse;
                // the default namespaceid = 0
            }
            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                if (!uint.TryParse(_recv["sdkrevision"], out session.PlayerInfo.SDKRevision))
                    _errorCode = GPErrorCode.Parse;
            }
            if (_recv.ContainsKey("gamename"))
            {
                session.PlayerInfo.GameName = _recv["gamename"];
            }
        }

        protected override void ConstructResponse(GPCMSession session)
        {
            session.PlayerInfo.SessionKey
                = _crc.ComputeChecksum(session.PlayerInfo.Nick + session.PlayerInfo.NamespaceID);

            string responseProof = ChallengeProof.GenerateProof
            (
                session.PlayerInfo.UserData,
                session.PlayerInfo.LoginType,
                session.PlayerInfo.PartnerID,
                session.PlayerInfo.ServerChallenge,
                session.PlayerInfo.UserChallenge,
                session.PlayerInfo.PasswordHash
            );

            _sendingBuffer = @"\lc\2\sesskey\" + session.PlayerInfo.SessionKey;
            _sendingBuffer += @"\proof\" + responseProof;
            _sendingBuffer += @"\userid\" + session.PlayerInfo.Userid;
            _sendingBuffer += @"\profileid\" + session.PlayerInfo.Profileid;

            if (session.PlayerInfo.LoginType != LoginType.Nick)
                _sendingBuffer += @"\uniquenick\" + session.PlayerInfo.UniqueNick;

            _sendingBuffer += @"\lt\" + session.Id.ToString().Replace("-", "").Substring(0, 22) + "__";
            _sendingBuffer += @"\id\" + _operationID + @"\final\";

            session.PlayerInfo.LoginProcess = LoginStatus.Completed;
        }

        protected override void DataBaseOperation(GPCMSession session)
        {

            switch (session.PlayerInfo.LoginType)
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

            if (!session.PlayerInfo.IsEmailVerified)
            {
                _errorCode = GPErrorCode.LoginBadEmail;
                return;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (session.PlayerInfo.IsBlocked)
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
                            where u.Email == session.PlayerInfo.Email
                            select u.Userid;

                if (email.Count() == 0)
                {
                    _errorCode = GPErrorCode.LoginBadEmail;
                }

                //Grab information from database
                var info = from u in db.Users
                           from p in db.Profiles
                           from n in db.Namespaces
                           where u.Email == session.PlayerInfo.Email && p.Nick == session.PlayerInfo.Nick && n.Namespaceid == session.PlayerInfo.NamespaceID
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
                    session.PlayerInfo.Userid = info.First().userid;
                    session.PlayerInfo.Profileid = info.First().profileid;
                    session.PlayerInfo.UniqueNick = info.First().uniquenick;
                    session.PlayerInfo.PasswordHash = info.First().password;
                    session.PlayerInfo.IsEmailVerified = info.First().emailVerified;
                    session.PlayerInfo.IsBlocked = info.First().blocked;
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
                var info = from n in db.Namespaces
                           join p in db.Profiles on n.Profileid equals p.Profileid
                           join u in db.Users on p.Userid equals u.Userid
                           where n.Uniquenick == session.PlayerInfo.UniqueNick
                           && n.Namespaceid == session.PlayerInfo.NamespaceID
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
                    session.PlayerInfo.Userid = info.First().userid;
                    session.PlayerInfo.Profileid = info.First().profileid;
                    session.PlayerInfo.UniqueNick = info.First().uniquenick;
                    session.PlayerInfo.PasswordHash = info.First().password;
                    session.PlayerInfo.IsEmailVerified = info.First().emailVerified;
                    session.PlayerInfo.IsBlocked = info.First().blocked;
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
                           join n in db.Namespaces on p.Profileid equals n.Profileid
                           where n.Authtoken == session.PlayerInfo.AuthToken && n.Partnerid == session.PlayerInfo.PartnerID
                           && n.Namespaceid == session.PlayerInfo.NamespaceID
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
                    session.PlayerInfo.Userid = info.First().userid;
                    session.PlayerInfo.Profileid = info.First().profileid;
                    session.PlayerInfo.UniqueNick = info.First().uniquenick;
                    session.PlayerInfo.PasswordHash = info.First().password;
                    session.PlayerInfo.IsEmailVerified = info.First().emailVerified;
                    session.PlayerInfo.IsBlocked = info.First().blocked;
                    session.PlayerInfo.Nick = info.First().nick;
                    session.PlayerInfo.Email = info.First().email;
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
            //SDKRevision.Switch(session);
        }
        protected bool IsChallengeCorrect(GPCMSession session)
        {
            string response = ChallengeProof.GenerateProof
                (
                session.PlayerInfo.UserData,
                session.PlayerInfo.LoginType,
                session.PlayerInfo.PartnerID,
                session.PlayerInfo.UserChallenge,
                session.PlayerInfo.ServerChallenge,
                session.PlayerInfo.PasswordHash
                );
            if (_recv["response"] == response)
            {
                return true;
            }
            return false;
        }
    }
}
