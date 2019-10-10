using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using PresenceConnectionManager.Application;
using PresenceConnectionManager.DatabaseQuery;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Structures;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PresenceConnectionManager.Handler
{
    public class LoginHandler
    {
        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        protected static Crc16 Crc = new Crc16(Crc16Mode.Standard);
        private static uint partnerID;

        /// <summary>
        /// This method verifies the login information sent by
        /// the session, and returns encrypted data for the session
        /// to verify as well
        /// </summary>
        public static void ProcessLogin(GPCMSession session, Dictionary<string, string> recv)
        {
            
            if (IsContainAllKeys(recv) != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.General, "Invalid response received from the session!");
                session.DisconnectByReason(DisconnectReason.InvalidLoginQuery);
                return;
            }
            
            if (recv.ContainsKey("uniquenick")&&recv.ContainsKey("namespaceid"))
            {
                UniquenickLogin(recv);
            }
            else if (recv.ContainsKey("authtoken"))
            {
                AuthtokenLogin(recv);
            }
            else if (recv.ContainsKey("user"))
            {                
                NoUniquenickLogin(recv);
            }
            else
            {
                session.ToLog("Invalid login method!!");
                //session.Disconnect();
            }


            // Parse the partnerid, required since it changes the challenge for Unique nick and User login
            ParseRequestToPlayerInfo(session, recv, ref partnerID);


            try
            {
                // Try and fetch the user from the database

                Dictionary<string, object> queryResult;

                try
                {
                    if (session.PlayerInfo.PlayerUniqueNick.Length > 0)
                    {
                        queryResult = LoginQuery.GetUserFromUniqueNick(recv);
                    }
                    else if (session.PlayerInfo.PlayerAuthToken.Length > 0)
                    {
                        //TODO! Add the database entry
                        GameSpyUtils.SendGPError(session, GPErrorCode.General, "AuthToken is not supported yet");
                        return;
                    }
                    else
                        queryResult = LoginQuery.GetUserFromNickAndEmail(recv);
                }
                catch (Exception ex)
                {
                    LogWriter.Log.WriteException(ex);
                    GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
                    return;
                }



                //if no match found we disconnect the game
                if (queryResult == null)
                {
                    if (session.PlayerInfo.PlayerUniqueNick.Length > 0)
                    {
                        GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "The uniquenick provided is incorrect!");
                    }
                    else
                    {
                        GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "The information provided is incorrect!");
                    }
                    session.DisconnectByReason(DisconnectReason.InvalidUsername);
                    return;
                }

                // Check if user is banned
                string msg;
                DisconnectReason reason;
                GPErrorCode error = CheckUsersAccountAvailability(queryResult, out msg, out reason);
                if (error != GPErrorCode.NoError)
                {
                    GameSpyUtils.SendGPError(session, error, msg);
                    session.DisconnectByReason(reason);
                    return;
                }

                // we finally set the player variables and return challengeData
                string challengeData = SetPlayerInfo(session, queryResult, recv);
                string sendingBuffer;
                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                if (recv["response"] == GenerateProof(recv["challenge"], session.ServerChallengeKey, challengeData, session.PlayerInfo.PlayerAuthToken.Length > 0 ? 0 : partnerID, session.PlayerInfo))
                {
                    // Create session key
                    session.SessionKey = Crc.ComputeChecksum(session.PlayerInfo.PlayerUniqueNick);

                    //actually we should store sesskey in database at namespace table, when we want someone's profile we just 
                    //access to the sesskey to find the uniquenick for particular game
                    LoginQuery.UpdateSessionKey(recv, session.SessionKey, session.PlayerInfo);

                    // Password is correct
                    sendingBuffer = string.Format(@"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        session.SessionKey,
                        GenerateProof(session.ServerChallengeKey, recv["challenge"], challengeData, session.PlayerInfo.PlayerAuthToken.Length > 0 ? 0 : partnerID, session.PlayerInfo), // Do this again, Params are reversed!
                        session.PlayerInfo.PlayerId,
                        session.PlayerInfo.PlayerUniqueNick,
                        // Generate LT whatever that is (some sort of random string, 22 chars long)
                        GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex)
                        );
                    //Send response to session
                    session.Send(sendingBuffer);
                    // Log Incoming Connections
                    //LogWriter.Log.Write(LogLevel.Info, "{0,-8} [Login] {1} - {2} - {3}", session.ServerName, session.PlayerInfo.PlayerNick, session.PlayerInfo.PlayerId, RemoteEndPoint);
                    //string statusString = string.Format(" [Login Success!] Nick:{0} - Profileid:{1} - IP:{2}", session.PlayerInfo.PlayerNick, session.PlayerInfo.PlayerId, session.RemoteEndPoint);
                    session.StatusToLog("Login Success", session.PlayerInfo.PlayerNick, session.PlayerInfo.PlayerId, (IPEndPoint)session.Socket.RemoteEndPoint, null);
                    // Update status last, and call success login
                    session.PlayerInfo.LoginStatus = LoginStatus.Completed;
                    session.PlayerInfo.PlayerStatus = PlayerStatus.Online;
                    session.PlayerInfo.PlayerStatusString = "Online";
                    session.PlayerInfo.PlayerStatusLocation = "";
                    session.CompletedLoginProcess = true;

                    SendBuddiesHandler.HandleSendBuddies(session, recv);
                }
                else
                {
                    // Log Incoming Connection
                    string statusString = string.Format(@"[Login Failed!] Nick:{0} - Profileid:{1} - IP:{2}", session.PlayerInfo.PlayerNick, session.PlayerInfo.PlayerId, session.Socket.RemoteEndPoint);
                    session.ToLog(LogLevel.Info, statusString);
                    // Password is incorrect with database value.
                    session.Send(@"\error\\err\260\fatal\\errmsg\The password provided is incorrect.\id\1\final\");
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

        private static void NoUniquenickLogin(Dictionary<string, string> recv)
        {
            ProcessNickAndEmail(recv);

            if (!recv.ContainsKey("namespaceid"))
            {
                recv.Add("namespaceid", "0");
            }

            throw new NotImplementedException();
        }

        private static void AuthtokenLogin(Dictionary<string, string> recv)
        {
            throw new NotImplementedException();
        }

        private static void UniquenickLogin(Dictionary<string,string> recv)
        {
            if (recv["namespaceid"]==" 1")
            {
                DefaultNamespaceLogin(recv);
            }
            if ( recv["namespaceid"] != " 1")
            {
                CustomNamespaceLogin(recv);
            }
        }

        private static void DefaultNamespaceLogin(Dictionary<string, string> recv)
        {
            //this method is only for GameSpy Arcade login
            throw new NotImplementedException();
        }

        private static void CustomNamespaceLogin(Dictionary<string, string> recv)
        {
            //this method is for games login
            throw new NotImplementedException();
        }

        private static void ProcessNickAndEmail(Dictionary<string, string> recv)
        {
            if (recv.ContainsKey("user"))
            {
                // "User" is <nickname>@<email>
                string user = recv["user"];
                int Pos = user.IndexOf('@');
                //we add the nick and email to dictionary
                string nick = user.Substring(0, Pos);
                string email = user.Substring(Pos + 1);
                recv.Add("nick", nick);
                recv.Add("email", email);
            }
        }
        private static GPErrorCode IsContainAllKeys(Dictionary<string, string> recv)
        {           
          
                // Make sure we have all the required data to process this login
                if (!recv.ContainsKey("challenge") || !recv.ContainsKey("response"))
            {
                return GPErrorCode.Parse;
            }
            else
            {
                return GPErrorCode.NoError;
            }
            
        }

        private static void ParseRequestToPlayerInfo(GPCMSession session, Dictionary<string, string> recv, ref uint partnerID)
        {

            if (recv.ContainsKey("partnerid"))
            {
                partnerID = Convert.ToUInt32(recv["partnerid"]);
                //partnerID = 0;
            }

            // Parse the 3 login types information
            if (recv.ContainsKey("uniquenick"))
            {
                session.PlayerInfo.PlayerUniqueNick = recv["uniquenick"];
            }
            else if (recv.ContainsKey("authtoken"))
            {
                session.PlayerInfo.PlayerAuthToken = recv["authtoken"];
            }
            else if (recv.ContainsKey("user"))
            {
                // "User" is <nickname>@<email>
                //string user = recv["user"];
                //int Pos = user.IndexOf('@');
                ////we add the nick and email to dictionary
                //string nick = user.Substring(0, Pos);
                //string email = user.Substring(Pos + 1);
                //recv.Add("nick", nick);
                //recv.Add("email", email);

                session.PlayerInfo.PlayerNick = recv["nick"];
                session.PlayerInfo.PlayerEmail = recv["email"];
            }
        }
        private string ConstructLoginResponseChallenge()
        {
            throw new NotImplementedException();
        }
        private static GPErrorCode CheckUsersAccountAvailability(Dictionary<string, object> queryResult, out string msg, out DisconnectReason reason)
        {
            PlayerStatus currentPlayerStatus;
            UserStatus currentUserStatus;

            if (!Enum.TryParse(queryResult["statuscode"].ToString(), out currentPlayerStatus))
            {
                msg = "Invalid player data! Please contact an administrator.";
                reason = DisconnectReason.InvalidPlayer;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //session.DisconnectByReason(DisconnectReason.InvalidPlayer);
            }
            if (!Enum.TryParse(queryResult["userstatus"].ToString(), out currentUserStatus))
            {
                msg = "Invalid player data! Please contact an administrator.";
                reason = DisconnectReason.InvalidPlayer;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //session.DisconnectByReason(DisconnectReason.InvalidPlayer);
                //return;
            }
            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (currentPlayerStatus == PlayerStatus.Banned)
            {
                msg = "Your profile has been permanently suspended.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your profile has been permanently suspended.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (currentUserStatus == UserStatus.Created)
            {
                msg = "Your account is not verified. Please check your email inbox and verify the account.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;

                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your account is not verified. Please check your email inbox and verify the account.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (currentUserStatus == UserStatus.Banned)
            {
                msg = "Your account has been permanently suspended.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(session, GPErrorCode.LoginBadUniquenick, "Your account has been permanently suspended.");
                //session.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }

            msg = "No error";
            reason = DisconnectReason.NormalLogout;
            return GPErrorCode.NoError;
        }

        private static string SetPlayerInfo(GPCMSession session, Dictionary<string, object> queryResult, Dictionary<string, string> recv)
        {
            session.PlayerInfo.PlayerId = uint.Parse(queryResult["profileid"].ToString());
            session.PlayerInfo.PasswordHash = queryResult["password"].ToString().ToLowerInvariant();
            session.PlayerInfo.PlayerCountryCode = queryResult["countrycode"].ToString();
            session.PlayerInfo.PlayerFirstName = queryResult["firstname"].ToString();
            session.PlayerInfo.PlayerLastName = queryResult["lastname"].ToString();
            session.PlayerInfo.PlayerICQ = int.Parse(queryResult["icq"].ToString());
            session.PlayerInfo.PlayerHomepage = queryResult["homepage"].ToString();
            session.PlayerInfo.PlayerZIPCode = queryResult["zipcode"].ToString();
            session.PlayerInfo.PlayerLocation = queryResult["location"].ToString();
            session.PlayerInfo.PlayerAim = queryResult["aim"].ToString();
            session.PlayerInfo.PlayerOwnership = int.Parse(queryResult["ownership1"].ToString());
            session.PlayerInfo.PlayerOccupation = int.Parse(queryResult["occupationid"].ToString());
            session.PlayerInfo.PlayerIndustryID = int.Parse(queryResult["industryid"].ToString());
            session.PlayerInfo.PlayerIncomeID = int.Parse(queryResult["incomeid"].ToString());
            session.PlayerInfo.PlayerMarried = int.Parse(queryResult["marriedid"].ToString());
            session.PlayerInfo.PlayerChildCount = int.Parse(queryResult["childcount"].ToString());
            session.PlayerInfo.PlayerConnectionType = int.Parse(queryResult["connectiontype"].ToString());
            session.PlayerInfo.PlayerPicture = int.Parse(queryResult["picture"].ToString());
            session.PlayerInfo.PlayerInterests = int.Parse(queryResult["interests1"].ToString());
            session.PlayerInfo.PlayerBirthday = ushort.Parse(queryResult["birthday"].ToString());
            session.PlayerInfo.PlayerBirthmonth = ushort.Parse(queryResult["birthmonth"].ToString());
            session.PlayerInfo.PlayerBirthyear = ushort.Parse(queryResult["birthyear"].ToString());

            PlayerSexType playerSexType;
            if (!Enum.TryParse(queryResult["sex"].ToString().ToUpper(), out playerSexType))
                session.PlayerInfo.PlayerSex = PlayerSexType.PAT;
            else
                session.PlayerInfo.PlayerSex = playerSexType;

            session.PlayerInfo.PlayerLatitude = float.Parse(queryResult["latitude"].ToString());
            session.PlayerInfo.PlayerLongitude = float.Parse(queryResult["longitude"].ToString());

            PublicMasks mask;
            if (!Enum.TryParse(queryResult["publicmask"].ToString(), out mask))
                session.PlayerInfo.PlayerPublicMask = PublicMasks.MASK_ALL;
            else
                session.PlayerInfo.PlayerPublicMask = mask;
            string challengeData;

            if (session.PlayerInfo.PlayerUniqueNick.Length > 0)
            {
                session.PlayerInfo.PlayerEmail = queryResult["email"].ToString();
                session.PlayerInfo.PlayerNick = queryResult["nick"].ToString();
                challengeData = session.PlayerInfo.PlayerUniqueNick;
                return challengeData;
            }
            else if (session.PlayerInfo.PlayerAuthToken.Length > 0)
            {
                session.PlayerInfo.PlayerEmail = queryResult["email"].ToString();
                session.PlayerInfo.PlayerNick = queryResult["nick"].ToString();
                session.PlayerInfo.PlayerUniqueNick = queryResult["uniquenick"].ToString();
                challengeData = session.PlayerInfo.PlayerAuthToken;
                return challengeData;
            }
            else
            {
                session.PlayerInfo.PlayerUniqueNick = queryResult["uniquenick"].ToString();
                challengeData = recv["user"];
                return challengeData;
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
        private static string GenerateProof(string challenge1, string challenge2, string userdata, uint partnerid, GPCMPlayerInfo playerinfo)
        {
            string realUserData = userdata;

            if (partnerid != (uint)PartnerID.Gamespy)
            {
                realUserData = string.Format("{0}@{1}", partnerid, userdata);
            }

            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(playerinfo.PasswordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
            HashString.Append(challenge1);
            HashString.Append(challenge2);
            HashString.Append(playerinfo.PasswordHash);
            return HashString.ToString().GetMD5Hash();
        }



    }
    
}
