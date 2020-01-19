using System;
using System.Collections.Generic;
using GameSpyLib.Common;
using GameSpyLib.Encryption;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.Login.Misc;
using PresenceConnectionManager.Handler.General.SDKExtendFeature;

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
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                return;
            }
            if (_recv.ContainsKey("partnerid"))
            {
                if (!uint.TryParse(_recv["partnerid"], out session.PlayerInfo.PartnerID))
                {
                    _errorCode = GPErrorCode.Parse;
                    session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                }
            }

            ParseDataBasedOnLoginType(session);

            ParseOtherData(session);


        }

        protected override void ConstructResponse(GPCMSession session)
        {
            session.PlayerInfo.SessionKey = _crc.ComputeChecksum(session.PlayerInfo.Nick + session.PlayerInfo.NamespaceID);
            string responseProof = ChallengeProof.GenerateProof
                (
                session.PlayerInfo,
                session.PlayerInfo.UserData,
                session.PlayerInfo.LoginType,
                session.PlayerInfo.PartnerID,
                session.PlayerInfo.ServerChallenge,
                session.PlayerInfo.UserChallenge,
                _result[0]["password"].ToString()
                );

            //string random = GameSpyRandom.GenerateRandomString(22, GameSpyRandom.StringType.Hex);
            //// Password is correct
            //_sendingBuffer = string.Format(
            //    @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
            //    session.PlayerInfo.SessionKey,
            //    responseProof,
            //    _result[0]["profileid"],
            //    _result[0]["uniquenick"],
            //    // Generate LT whatever that is (some sort of random string, 22 chars long)
            //    random
            //    );

            _sendingBuffer = @"\lc\2\sesskey\" + session.PlayerInfo.SessionKey;
            _sendingBuffer += @"\proof\" + responseProof;
            _sendingBuffer += @"\userid\" + _result[0]["userid"];
            _sendingBuffer += @"\profileid\" + _result[0]["profileid"];

            if (session.PlayerInfo.LoginType == LoginType.Uniquenick)
                _sendingBuffer += @"\uniquenick\" + _result[0]["uniquenick"];

            _sendingBuffer += @"\lt\" + session.Id.ToString().Replace("-", "").Substring(0, 22) + "__";
            _sendingBuffer += @"\id\" + _operationID + @"\final\";

            session.PlayerInfo.LoginProcess = LoginStatus.Completed;
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            CheckUserBasedOnLoginType(session);
            if (_result == null)
            {
                _errorCode = GPErrorCode.DatabaseError;
                return;
            }

            if (!CheckAccount(session))
            {
                return;
            }

            session.PlayerInfo.PasswordHash = _result[0]["password"].ToString();

            if (!IsChallengeCorrect(session))
            {
                //TODO check the challenge response correctness
                _errorCode = GPErrorCode.LoginBadLoginTicket;
            }

        }

        protected override void Response(GPCMSession session)
        {
            base.Response(session);
            SDKRevision.Switch(session, _recv);
        }

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
                    // Ignore malformed user
                    // Pos == -1 : Not found
                    // Pos < 1 : @ or @example
                    // Pos + 1 >= Length : example@
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
                // the default partnerid will be (uint)gamespy.
                session.PlayerInfo.PartnerID = Convert.ToUInt32(_recv["partnerid"]);
            }
            if (_recv.ContainsKey("namespaceid"))
            {
                // the default namespaceid = 0
                session.PlayerInfo.NamespaceID = Convert.ToUInt32(_recv["namespaceid"]);
            }
            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                session.PlayerInfo.SDKRevision = Convert.ToUInt32(_recv["sdkrevision"]);
            }
        }

        private void CheckUserBasedOnLoginType(GPCMSession session)
        {
            switch (session.PlayerInfo.LoginType)
            {
                case LoginType.Nick:
                    _result = LoginQuery.GetUserFromNickAndEmail(session.PlayerInfo.NamespaceID, session.PlayerInfo.Nick, session.PlayerInfo.Email);
                    break;
                case LoginType.Uniquenick:
                    _result = LoginQuery.GetUserFromUniqueNick(session.PlayerInfo.UniqueNick, session.PlayerInfo.NamespaceID);
                    break;
                case LoginType.AuthToken:
                    //TODO
                    break;
            }
        }

        protected bool IsChallengeCorrect(GPCMSession session)
        {
            string response = ChallengeProof.GenerateProof
                (
                session.PlayerInfo,
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

        protected bool CheckAccount(GPCMSession session)
        {
            bool isVerified = Convert.ToBoolean(_result[0]["emailverified"]);
            bool isBanned = Convert.ToBoolean(_result[0]["banned"]);
            if (!isVerified)
            {
                _errorCode = GPErrorCode.LoginBadEmail;
                return false;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (isBanned)
            {
                _errorCode = GPErrorCode.LoginProfileDeleted;
                return false;
            }

            return true;
        }
    }
}
