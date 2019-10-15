using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.SDKExtendFeature;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler.Login
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

        private static GPErrorCode _errorCode;

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

            IsContainAllKeys();
            if (_errorCode != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _errorCode, _errorMsg);
                session.DisconnectByReason(_disconnectReason);
                return;
            }

            PreProcessForLogin(session);

            if (recv.ContainsKey("uniquenick"))
            {
                UniquenickLoginMethod(session);
            }
            else if (recv.ContainsKey("authtoken"))
            {
                AuthTokenLoginMethod(session);
            }
            else if (recv.ContainsKey("user"))
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
            CheckDatabaseResult();
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

            SDKRevision.Switch(session,_recv);
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
                session.PlayerInfo.SDKRevision =Convert.ToUInt32( _recv["sdkrevision"]);
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
            session.PlayerInfo.Profileid = Convert.ToUInt32(_queryResult["profileid"]);
        }

        private static void UniquenickLoginMethod(GPCMSession session)
        {
            if (LoginHandler._recv.ContainsKey("namespaceid"))
            {
                _queryResult = LoginQuery.GetUserFromUniqueNick(_recv);
                session.PlayerInfo.Profileid = Convert.ToUInt32(_queryResult["profileid"]);
            }
            else
            {
                _recv.Add("namespaceid", "0");
                session.PlayerInfo.Namespaceid = 0;
                _queryResult = LoginQuery.GetUserFromUniqueNick(_recv);
                session.PlayerInfo.Profileid = Convert.ToUInt32(_queryResult["profileid"]);
            }
        }
        #endregion

        private static void CheckDatabaseResult()
        {
            if (_queryResult == null)
            {
                if (_recv.ContainsKey("uniquenick"))
                {

                    _errorCode = GPErrorCode.LoginBadUniquenick;
                    _errorMsg = "The uniquenick provided is incorrect!";
                    _disconnectReason = DisconnectReason.InvalidUsername;
                }
                else
                {
                    _errorCode = GPErrorCode.LoginBadProfile;
                    _errorMsg = "The information provided is incorrect!";
                    _disconnectReason = DisconnectReason.InvalidUsername;
                }
            }
        }
        private static void CheckUsersAccountAvailability()
        {
            PlayerOnlineStatus onlineStatus;
            UserStatus userStatus;

            if (!Enum.TryParse(_queryResult["statuscode"].ToString(), out onlineStatus))
            {
                _errorMsg = "Invalid player data! Please contact an administrator.";
                _disconnectReason = DisconnectReason.InvalidPlayer;
                _errorCode = GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //session.DisconnectByReason(DisconnectReason.InvalidPlayer);
            }
            if (!Enum.TryParse(_queryResult["userstatus"].ToString(), out userStatus))
            {
                _errorMsg = "Invalid player data! Please contact an administrator.";
                _disconnectReason = DisconnectReason.InvalidPlayer;
                _errorCode = GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //session.DisconnectByReason(DisconnectReason.InvalidPlayer);
                //return;
            }
            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (onlineStatus == PlayerOnlineStatus.Banned)
            {
                _errorMsg = "Your profile has been permanently suspended.";
                _disconnectReason = DisconnectReason.PlayerIsBanned;
                _errorCode = GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your profile has been permanently suspended.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (userStatus == UserStatus.Created)
            {
                _errorMsg = "Your account is not verified. Please check your email inbox and verify the account.";
                _disconnectReason = DisconnectReason.PlayerIsBanned;
                _errorCode = GPErrorCode.LoginBadUniquenick;

                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your account is not verified. Please check your email inbox and verify the account.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (userStatus == UserStatus.Banned)
            {
                _errorMsg = "Your account has been permanently suspended.";
                _disconnectReason = DisconnectReason.PlayerIsBanned;
                _errorCode = GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your account has been permanently suspended.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }

            _errorMsg = "No error";
            _disconnectReason = DisconnectReason.NormalLogout;
            _errorCode = GPErrorCode.NoError;
        }
        public static void SendLoginResponseChallenge(GPCMSession session)
        {
            try
            {
                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                string response = GenerateProof(session,session.PlayerInfo.UserChallenge, session.PlayerInfo.ServerChallenge, _queryResult["password"].ToString());
                if (_recv["response"] == response)
                {
                    // Create session key
                    session.PlayerInfo.SessionKey = _crc.ComputeChecksum(_queryResult["uniquenick"].ToString()+_recv["namespaceid"]);

                    //actually we should store sesskey in database at namespace table, when we want someone's profile we just 
                    //access to the sesskey to find the uniquenick for particular game
                    LoginQuery.UpdateSessionKey(_queryResult, session.PlayerInfo.Namespaceid, session.PlayerInfo.SessionKey, session.Id);

                    string responseProof = GenerateProof(session,session.PlayerInfo.ServerChallenge, session.PlayerInfo.UserChallenge, _queryResult["password"].ToString());

                    // Password is correct
                    _sendingBuffer = 
                        string.Format(@"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        session.PlayerInfo.SessionKey,
                        //GenerateProof(Session.ServerChallengeKey, Recv["challenge"], Recv["user"], Recv["authtoken"]?.Length > 0 ? 0 : _partnerid, _originalPassword), // Do this again, Params are reversed!
                        responseProof,
                        _queryResult["profileid"],
                        _queryResult["uniquenick"],
                        // Generate LT whatever that is (some sort of random string, 22 chars long)
                        GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex));
                    session.PlayerInfo.LoginProcess = LoginStatus.Completed;
                    session.SendAsync(_sendingBuffer);
                }
                else
                {
                    // Log Incoming Connection
                    string statusString = string.Format(@"[Login Failed!] Nick:{0} - Profileid:{1} - IP:{2}", session.PlayerInfo.User, _queryResult["profileid"], session.Socket.RemoteEndPoint);
                    session.ToLog(LogLevel.Info, statusString);
                    // Password is incorrect with database value.
                    GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadPassword, "The password provided is incorrect");
                    session.DisconnectByReason(DisconnectReason.InvalidPassword);
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
        private static string GenerateProof(GPCMSession session,string challenge1, string challenge2, string passwordHash)
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
