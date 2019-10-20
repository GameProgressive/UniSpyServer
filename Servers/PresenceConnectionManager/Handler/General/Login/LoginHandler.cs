using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.General.Login.Query;
using PresenceConnectionManager.Handler.General.SDKExtendFeature;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.General.Login
{
    public class LoginHandler
    {
        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        private static Crc16 _crc = new Crc16(Crc16Mode.Standard);
        /// <summary>
        /// save the request we received, convience for us to use
        /// </summary>
        private static Dictionary<string, string> _recv;
        /// <summary>
        /// store the query result from database
        /// </summary>
        private static Dictionary<string, object> _queryResult;
        /// <summary>
        /// store the information we need to send to user
        /// </summary>
        private static string _sendingBuffer;

        private static GPErrorCode _errorCode = GPErrorCode.NoError;

        private static string _errorMsg;

        private static DisconnectReason _disconnectReason;
        /// <summary>
        /// This method verifies the login information sent by
        /// the session, and returns encrypted data for the session
        /// to verify as well
        /// </summary>
        public static void ProcessLogin(GPCMSession session, Dictionary<string, string> recv)
        {
            _recv = recv;
            _queryResult = null;
            _sendingBuffer = "";
            _errorCode = GPErrorCode.NoError;
            _errorMsg = "";
            _disconnectReason = DisconnectReason.NormalLogout;

            IsContainAllKeys();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(_disconnectReason);
                return;
            }

            PreProcessForLogin(session);

            if (_recv.ContainsKey("uniquenick"))
            {
                UniquenickLoginMethod(session);
            }
            else if (_recv.ContainsKey("authtoken"))
            {
                AuthTokenLoginMethod(session);
            }
            else if (_recv.ContainsKey("user"))
            {
                NoUniquenickLoginMethod(session);
            }
            else
            {
                session.ToLog("Invalid login method!!");
                session.DisconnectByReason(DisconnectReason.GeneralError);
                return;
            }
            //if no match found we disconnect the session
            CheckDatabaseResult(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(_disconnectReason);
                return;
            }

            // Check if user is banned
            CheckUsersAccountAvailability();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(_disconnectReason);
                return;
            }

            SendLoginResponseChallenge(session);
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(_disconnectReason);
                return;
            }

            SDKRevision.Switch(session, _recv);


        }



        private static void IsContainAllKeys()
        {
            // Make sure we have all the required data to process this login
            if (!_recv.ContainsKey("challenge") || !_recv.ContainsKey("response"))
            {
                _errorCode = GPErrorCode.Parse;
                _errorMsg = "Parsing error";
                _disconnectReason = DisconnectReason.InvalidLoginQuery;
            }
            else
            {
                _errorCode = GPErrorCode.NoError;
            }
        }
        /// <summary>
        /// We make a pre process for the data we received
        /// </summary>
        public static void PreProcessForLogin(GPCMSession session)
        {
            if (_recv.ContainsKey("user"))
            {
                // "User" is <nickname>@<email>
                string user = _recv["user"];
                int Pos = user.IndexOf('@');
                //we add the nick and email to dictionary
                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);
                session.PlayerInfo.Nick = nick;
                session.PlayerInfo.Email = email;
                //improve it later
                _recv.Add("nick", nick);
                _recv.Add("email", email);
            }
            session.PlayerInfo.UserChallenge = _recv["challenge"];

            if (_recv.ContainsKey("uniquenick"))
            {
                session.PlayerInfo.UniqueNick = _recv["uniquenick"];
                session.PlayerInfo.UserData = session.PlayerInfo.UniqueNick;
            }
            else if (_recv.ContainsKey("authtoken"))
            {
                session.PlayerInfo.AuthToken = _recv["authtoken"].ToString();
                session.PlayerInfo.UserData = session.PlayerInfo.AuthToken;
            }
            else
            {
                session.PlayerInfo.User = _recv["user"].ToString();
                session.PlayerInfo.UserData = session.PlayerInfo.User;
            }
            if (_recv.ContainsKey("partnerid"))
            {
                session.PlayerInfo.Partnerid = Convert.ToUInt32(_recv["partnerid"]);
            }
            else
            {
                session.PlayerInfo.Partnerid = 0;
            }
            if (_recv.ContainsKey("namespaceid"))
            {
                session.PlayerInfo.Namespaceid = Convert.ToUInt32(_recv["namespaceid"]);
            }
            else
            {
                session.PlayerInfo.Namespaceid = 0;
            }
            //store sdkrevision
            if (_recv.ContainsKey("sdkrevision"))
            {
                session.PlayerInfo.SDKRevision = Convert.ToUInt32(_recv["sdkrevision"]);
            }

        }

        #region Login Methods
        private static void AuthTokenLoginMethod(GPCMSession session)
        {
            session.DisconnectByReason(DisconnectReason.ForcedLogout);
            throw new NotImplementedException();
        }

        private static void NoUniquenickLoginMethod(GPCMSession session)
        {
            if (!_recv.ContainsKey("namespaceid"))
            {
                _recv.Add("namespaceid", "0");
            }
            _queryResult = LoginQuery.GetUserFromNickAndEmail(_recv);
        }

        private static void UniquenickLoginMethod(GPCMSession session)
        {
            if (!_recv.ContainsKey("namespaceid"))
            {
                _recv.Add("namespaceid", "0");
                session.PlayerInfo.Namespaceid = 0;
            }
            _queryResult = LoginQuery.GetUserFromUniqueNick(_recv);
        }
        #endregion

        private static void CheckDatabaseResult(GPCMSession session)
        {
            if (_queryResult == null)
            {
                if (_recv.ContainsKey("uniquenick"))
                {
                    _errorCode = GPErrorCode.LoginBadUniquenick;
                    _errorMsg = "The uniquenick provided is incorrect!";
                    _disconnectReason = DisconnectReason.InvalidUsername;
                }
                else if (_recv.ContainsKey("nick"))
                {
                    _errorCode = GPErrorCode.LoginBadNick;
                    _errorMsg = "The information provided is incorrect!";
                    _disconnectReason = DisconnectReason.InvalidUsername;
                }
                else//authtoken error
                {
                    _errorCode = GPErrorCode.AuthAddBadForm;
                    _errorMsg = "The information provided is incorrect!";
                    _disconnectReason = DisconnectReason.InvalidLoginQuery;
                }
            }
            else
            {
                //parse profileid to playerinfo
                session.PlayerInfo.Profileid = Convert.ToUInt32(_queryResult["profileid"]);
            }
        }
        private static void CheckUsersAccountAvailability()
        {
            bool isVerified = Convert.ToBoolean(_queryResult["emailverified"]);
            bool isBanned = Convert.ToBoolean(_queryResult["banned"]);
            if (!isVerified)
            {
                _errorMsg = "Your account is not verified. Please check your email inbox and verify the account.";
                _disconnectReason = DisconnectReason.InvalidPlayer;
                _errorCode = GPErrorCode.LoginBadProfile;
            }

            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (isBanned)
            {
                _errorMsg = "Your profile has been permanently suspended.";
                _disconnectReason = DisconnectReason.PlayerIsBanned;
                _errorCode = GPErrorCode.LoginProfileDeleted;
            }
        }
        public static void SendLoginResponseChallenge(GPCMSession session)
        {
            try
            {
                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                string response = GenerateProof(session, session.PlayerInfo.UserChallenge, session.PlayerInfo.ServerChallenge, _queryResult["password"].ToString());
                if (_recv["response"] == response)
                {
                    // Create session key
                    session.PlayerInfo.SessionKey = _crc.ComputeChecksum(_queryResult["uniquenick"] + _recv["namespaceid"]);

                    //actually we should store sesskey in database at namespace table, when we want someone's profile we just 
                    //access to the sesskey to find the uniquenick for particular game
                    LoginQuery.UpdateSessionKey(session.PlayerInfo.Profileid, session.PlayerInfo.Namespaceid, session.PlayerInfo.SessionKey, session.Id);

                    string responseProof = GenerateProof(session, session.PlayerInfo.ServerChallenge, session.PlayerInfo.UserChallenge, _queryResult["password"].ToString());

                    string random = GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex);
                    // Password is correct
                    _sendingBuffer = string.Format(
                        @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        session.PlayerInfo.SessionKey,
                        responseProof,
                        _queryResult["profileid"],
                        _queryResult["uniquenick"],
                        // Generate LT whatever that is (some sort of random string, 22 chars long)
                        random
                        );

                    session.PlayerInfo.LoginProcess = LoginStatus.Completed;
                    session.SendAsync(_sendingBuffer);
                }
                else
                {
                    _errorCode = GPErrorCode.LoginBadPassword;
                    _errorMsg = "Password is not correct";
                    _disconnectReason = DisconnectReason.InvalidPassword;
                }
            }

            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.ToString(), LogLevel.Error);
                session.DisconnectByReason(DisconnectReason.GeneralError);
                return;
            }
        }

        /// <summary>
        /// Generates an MD5 hash, which is used to verify the sessions login information
        /// </summary>
        /// <param name="challenge1">First challenge key</param>
        /// <param name="challenge2">Second challenge key</param>
        /// <param name="userdata">The user data to append to the proof</param>
        /// <param name="partnerid">The partnerid to append</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the session sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        private static string GenerateProof(GPCMSession session, string challenge1, string challenge2, string passwordHash)
        {
            string realUserData = session.PlayerInfo.UserData;

            if (session.PlayerInfo.Partnerid != (uint)PartnerID.Gamespy)
            {
                realUserData = string.Format("{0}@{1}", session.PlayerInfo.Partnerid, session.PlayerInfo.UserData);
            }
            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(passwordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
            HashString.Append(challenge1);
            HashString.Append(challenge2);
            HashString.Append(passwordHash);
            return HashString.ToString().GetMD5Hash();
        }
    }

}
