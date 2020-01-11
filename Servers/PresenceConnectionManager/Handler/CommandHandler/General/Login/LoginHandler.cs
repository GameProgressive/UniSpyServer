using GameSpyLib.Common;
using GameSpyLib.Encryption;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using PresenceConnectionManager.Handler.Error;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.General.Login
{
    public class LoginHandler : GPCMHandlerBase
    {
        public LoginHandler( Dictionary<string, string> recv) : base( recv)
        {
        }

        Crc16 _crc = new Crc16(Crc16Mode.Standard);
        /// <summary>
        /// This method verifies the login information sent by
        /// the session, and returns encrypted data for the session
        /// to verify as well
        /// </summary>
        public override void Handle(GPCMSession session)
        {     
            CheckRequest(session);

            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            //if no match found we disconnect the session
            DataBaseOperation(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            // Check if user is banned
            CheckUsersAccountAvailability(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            SendLoginResponseChallenge(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                ErrorMsg.SendGPCMError(session, _errorCode, _operationID);
                session.DisconnectByReason(session.PlayerInfo.DisconReason);
                return;
            }

            SDKRevision.Switch(session, _recv);
        }

        protected override void CheckRequest(GPCMSession session)
        {
            // Make sure we have all the required data to process this login
            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                _errorCode = GPErrorCode.Parse;
                session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                return;
            }
            else
            {
                _errorCode = GPErrorCode.NoError;
            }


            if(!PreProcessForLogin(session))
            {
                _errorCode = GPErrorCode.Parse;
            }
        }
        /// <summary>
        /// We make a pre process for the data we received
        /// </summary>
        public bool PreProcessForLogin(GPCMSession session)
        {
            session.PlayerInfo.UserChallenge = _recv["challenge"];

            if (_recv.ContainsKey("uniquenick"))
            {
                session.PlayerInfo.UniqueNick = _recv["uniquenick"];
                session.PlayerInfo.UserData = session.PlayerInfo.UniqueNick;
                session.PlayerInfo.LoginType = LoginType.Uniquenick;
            }
            else if (_recv.ContainsKey("authtoken"))
            {
                session.PlayerInfo.AuthToken = _recv["authtoken"].ToString();
                session.PlayerInfo.UserData = session.PlayerInfo.AuthToken;
                session.PlayerInfo.LoginType = LoginType.AuthToken;
            }
            else
            {
                // "User" is <nickname>@<email>, it will be splitted in this function

                string user = _recv["user"].ToString();

                int Pos = user.IndexOf('@');
                if (Pos == -1 || Pos < 1 || (Pos + 1) >= user.Length)
                {
                    // Ignore malformed user
                    // Pos == -1 : Not found
                    // Pos < 1 : @ or @example
                    // Pos + 1 >= Length : example@
                    return false;
                }

                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);

                session.PlayerInfo.Nick = nick;
                session.PlayerInfo.Email = email;
                session.PlayerInfo.UserData = user;
                session.PlayerInfo.LoginType = LoginType.Nick;
            }

            if (_recv.ContainsKey("partnerid"))
            {
                session.PlayerInfo.PartnerID = Convert.ToUInt32(_recv["partnerid"]);
            }
            else
            {
                session.PlayerInfo.PartnerID = (uint)PartnerID.Gamespy; // Default partnerid: Gamespy
            }

            if (_recv.ContainsKey("namespaceid"))
            {
                session.PlayerInfo.NamespaceID = Convert.ToUInt32(_recv["namespaceid"]);
            }
            else
            {
                session.PlayerInfo.NamespaceID = 0; // Default namespaceid
            }

            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                session.PlayerInfo.SDKRevision = Convert.ToUInt32(_recv["sdkrevision"]);
            }

            return true;
        }

        protected override void DataBaseOperation(GPCMSession session)
        {
            switch(session.PlayerInfo.LoginType)
            {
                case LoginType.Uniquenick:
                    _result = LoginQuery.GetUserFromUniqueNick(session.PlayerInfo.UniqueNick, session.PlayerInfo.NamespaceID);
                    break;
                case LoginType.Nick:
                    _result = LoginQuery.GetUserFromNickAndEmail(session.PlayerInfo.NamespaceID, session.PlayerInfo.Nick, session.PlayerInfo.Email);
                    break;
                case LoginType.AuthToken:
                    ErrorMsg.SendGPCMError(session, GPErrorCode.Login, _operationID);
                    session.DisconnectByReason(DisconnectReason.ForcedLogout);
                    break;
                default:
                    _result = null;
                    return;
            }

            if (_result == null)
            {
                switch (session.PlayerInfo.LoginType)
                {
                    case LoginType.Uniquenick:
                        _errorCode = GPErrorCode.LoginBadUniquenick;
                        session.PlayerInfo.DisconReason = DisconnectReason.InvalidUsername;
                        break;

                    case LoginType.Nick:
                        _errorCode = GPErrorCode.LoginBadNick;
                        session.PlayerInfo.DisconReason = DisconnectReason.InvalidUsername;
                        break;

                    case LoginType.AuthToken:
                        _errorCode = GPErrorCode.AuthAddBadForm;
                        session.PlayerInfo.DisconReason = DisconnectReason.InvalidLoginQuery;
                        break;
                }
            }
            else
            {
                //parse profileid to playerinfo
                session.PlayerInfo.Profileid = Convert.ToUInt32(_result[0]["profileid"]);
            }
        }

        private void CheckUsersAccountAvailability(GPCMSession session)
        {
            bool isVerified = Convert.ToBoolean(_result[0]["emailverified"]);
            bool isBanned = Convert.ToBoolean(_result[0]["banned"]);
            if (!isVerified)
            {

                session.PlayerInfo.DisconReason = DisconnectReason.InvalidPlayer;
                _errorCode = GPErrorCode.LoginBadEmail;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (isBanned)
            {

                session.PlayerInfo.DisconReason = DisconnectReason.PlayerIsBanned;
                _errorCode = GPErrorCode.LoginProfileDeleted;
            }
        }

        public void SendLoginResponseChallenge(GPCMSession session)
        {
            try
            {
                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                string response = GenerateProof(session, session.PlayerInfo.UserChallenge, session.PlayerInfo.ServerChallenge, _result[0]["password"].ToString());
                if (_recv["response"] == response)
                {
                    // Create session key
                    session.PlayerInfo.SessionKey = _crc.ComputeChecksum(_result[0]["uniquenick"] + _recv["namespaceid"]);

                    //actually we should store sesskey in database at namespace table, when we want someone's profile we just 
                    //access to the sesskey to find the uniquenick for particular game
                    if (!LoginQuery.UpdateSessionKey(session.PlayerInfo.Profileid, session.PlayerInfo.NamespaceID, session.PlayerInfo.SessionKey, session.Id))
                    {
                        _errorCode = GPErrorCode.DatabaseError;

                        session.PlayerInfo.DisconReason = DisconnectReason.GeneralError;
                        return;
                    }

                    string responseProof = GenerateProof(session, session.PlayerInfo.ServerChallenge, session.PlayerInfo.UserChallenge, _result[0]["password"].ToString());

                    string random = GameSpyRandom.GenerateRandomString(22, GameSpyRandom.StringType.Hex);
                    // Password is correct
                    _sendingBuffer = string.Format(
                        @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        session.PlayerInfo.SessionKey,
                        responseProof,
                        _result[0]["profileid"],
                        _result[0]["uniquenick"],
                        // Generate LT whatever that is (some sort of random string, 22 chars long)
                        random
                        );

                    session.PlayerInfo.LoginProcess = LoginStatus.Completed;
                    session.SendAsync(_sendingBuffer);
                }
                else
                {
                    _errorCode = GPErrorCode.LoginBadPassword;

                    session.PlayerInfo.DisconReason = DisconnectReason.InvalidPassword;
                }
            }

            catch (Exception ex)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.General, "There was an unknown error.");
                LogWriter.Log.Write(ex.ToString(), LogLevel.Error);
                session.DisconnectByReason(DisconnectReason.GeneralError);
                return;
            }
        }

        /// <summary>
        /// Generates an MD5 hash, which is used to verify the sessions login information
        /// </summary>
        /// <param name="serverChallenge">First challenge key</param>
        /// <param name="clientChallenge">Second challenge key</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the session sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        private static string GenerateProof(GPCMSession session, string serverChallenge, string clientChallenge, string passwordHash)
        {
            string realUserData = session.PlayerInfo.UserData;

            // Auth token does not have partnerid append.
            if (session.PlayerInfo.PartnerID != (uint)PartnerID.Gamespy && session.PlayerInfo.LoginType != LoginType.AuthToken)
            {
                realUserData = string.Format("{0}@{1}", session.PlayerInfo.PartnerID, session.PlayerInfo.UserData);
            }

            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(passwordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
            HashString.Append(serverChallenge);
            HashString.Append(clientChallenge);
            HashString.Append(passwordHash);
            return HashString.ToString().GetMD5Hash();
        }
    }
}
