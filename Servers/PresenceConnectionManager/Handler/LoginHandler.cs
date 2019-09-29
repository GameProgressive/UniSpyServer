using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using PresenceConnectionManager.Application;
using PresenceConnectionManager.DatabaseQuery;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler
{
    public class LoginHandler
    {
        /// <summary>
        /// Our CRC16 object for generating Checksums
        /// </summary>
        protected static Crc16 Crc = new Crc16(Crc16Mode.Standard);
        /// <summary>
        /// This method verifies the login information sent by
        /// the client, and returns encrypted data for the client
        /// to verify as well
        /// </summary>
        public static void ProcessLogin(GPCMClient client, Dictionary<string, string> recv, GPCMConnectionUpdate OnSuccessfulLogin, GPCMStatusChanged OnStatusChanged)
        {
            uint partnerID = 0;
            // Make sure we have all the required data to process this login
            //if (!recv.ContainsKey("challenge") || !recv.ContainsKey("response"))
            //{
            //    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "Invalid response received from the client!");
            //    client.DisconnectByReason(DisconnectReason.InvalidLoginQuery);
            //    return;
            //}
            if (IsContainAllKeys(recv) != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "Invalid response received from the client!");
                client.DisconnectByReason(DisconnectReason.InvalidLoginQuery);
                return;
            }


            // Parse the partnerid, required since it changes the challenge for Unique nick and User login
            ParseRequestToPlayerInfo(client, recv, ref partnerID);


            // Dispose connection after use
            try
            {
                // Try and fetch the user from the database

                Dictionary<string, object> queryResult;

                try
                {
                    if (client.PlayerInfo.PlayerUniqueNick.Length > 0)
                    {
                        queryResult = LoginQuery.GetUserFromUniqueNick(recv);
                    }
                    else if (client.PlayerInfo.PlayerAuthToken.Length > 0)
                    {
                        //TODO! Add the database entry
                        GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "AuthToken is not supported yet");
                        return;
                    }
                    else
                        queryResult = LoginQuery.GetUserFromNickAndEmail(recv);
                }
                catch (Exception ex)
                {
                    LogWriter.Log.WriteException(ex);
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
                    return;
                }

                //if no match found we disconnect the game
                if (queryResult == null)
                {
                    if (client.PlayerInfo.PlayerUniqueNick.Length > 0)
                    {
                        GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "The uniquenick provided is incorrect!");
                    }

                    else
                    {
                        GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "The nick provided is incorrect!");
                    }
                    client.DisconnectByReason(DisconnectReason.InvalidUsername);
                    return;
                }

                // Check if user is banned
                string msg;
                DisconnectReason reason;
                GPErrorCode error = CheckUsersAccountAvailability(queryResult, out msg, out reason);
                if (error != GPErrorCode.NoError)
                {
                    GameSpyUtils.SendGPError(client.Stream, error, msg);
                    client.DisconnectByReason(reason);
                    return;
                }

                // we finally set the player variables and return challengeData
                string challengeData = SetPlayerInfo(client, queryResult, recv);
                string sendingBuffer;
                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                if (recv["response"] == GenerateProof(recv["challenge"], client.ServerChallengeKey, challengeData, client.PlayerInfo.PlayerAuthToken.Length > 0 ? 0 : partnerID, client.PlayerInfo))
                {
                    // Create session key
                    client.SessionKey = Crc.ComputeChecksum(client.PlayerInfo.PlayerUniqueNick);

                    //actually we should store sesskey in database at namespace table, when we want someone's profile we just 
                    //access to the sesskey to find the uniquenick for particular game
                    LoginQuery.UpdateSessionKey(recv, client.SessionKey, client.PlayerInfo);

                    // Password is correct
                    sendingBuffer = string.Format(@"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        client.SessionKey,
                        GenerateProof(client.ServerChallengeKey, recv["challenge"], challengeData, client.PlayerInfo.PlayerAuthToken.Length > 0 ? 0 : partnerID, client.PlayerInfo), // Do this again, Params are reversed!
                        client.PlayerInfo.PlayerId,
                        client.PlayerInfo.PlayerUniqueNick,
                        // Generate LT whatever that is (some sort of random string, 22 chars long)
                        GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex)
                        );
                    //Send response to client
                    client.Send(sendingBuffer);

                    // Log Incoming Connections
                    //LogWriter.Log.Write(LogLevel.Info, "{0,-8} [Login] {1} - {2} - {3}", client.Stream.ServerName, client.PlayerInfo.PlayerNick, client.PlayerInfo.PlayerId, RemoteEndPoint);
                    client.ToLog(LogLevel.Info, "Login", "Success", "Nick:{0} - Profileid:{1} - IP:{2}", client.PlayerInfo.PlayerNick, client.PlayerInfo.PlayerId, client.RemoteEndPoint);
                    // Update status last, and call success login
                    client.PlayerInfo.LoginStatus = LoginStatus.Completed;
                    client.PlayerInfo.PlayerStatus = PlayerStatus.Online;
                    client.PlayerInfo.PlayerStatusString = "Online";
                    client.PlayerInfo.PlayerStatusLocation = "";
                    client.CompletedLoginProcess = true;

                    OnSuccessfulLogin?.Invoke(client);
                    OnStatusChanged?.Invoke(client);
                    SendBuddiesHandler.HandleSendBuddies(client, recv);
                }
                else
                {
                    // Log Incoming Connection
                    client.ToLog(LogLevel.Info, "Login", "Failed", "{0} - {1} - {2}", client.PlayerInfo.PlayerNick, client.PlayerInfo.PlayerId, client.RemoteEndPoint);
                    // Password is incorrect with database value.
                    client.Send(@"\error\\err\260\fatal\\errmsg\The password provided is incorrect.\id\1\final\");
                    client.DisconnectByReason(DisconnectReason.InvalidPassword);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.ToString(), LogLevel.Error);
                client.DisconnectByReason(DisconnectReason.GeneralError);
                return;
            }
        }

        private static GPErrorCode IsContainAllKeys(Dictionary<string, string> recv)
        {
            if (!recv.ContainsKey("namespaceid"))
            {
                recv.Add("namespaceid", "0");
            }
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
            //    if (!recv.ContainsKey("uniquenick"))
            //{
            //    string email = recv["email"];
            //    int Pos = email.IndexOf('@');
            //    //we add the nick and email to dictionary
            //    string uniquenick = email.Substring(0, Pos);
            //    recv.Add("uniquenick", uniquenick);
            //}
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

        private static void ParseRequestToPlayerInfo(GPCMClient client, Dictionary<string, string> recv, ref uint partnerID)
        {

            if (recv.ContainsKey("partnerid"))
            {
                partnerID = Convert.ToUInt32(recv["partnerid"]);
                //partnerID = 0;
            }

            // Parse the 3 login types information
            if (recv.ContainsKey("uniquenick"))
            {
                client.PlayerInfo.PlayerUniqueNick = recv["uniquenick"];
            }
            else if (recv.ContainsKey("authtoken"))
            {
                client.PlayerInfo.PlayerAuthToken = recv["authtoken"];
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

                client.PlayerInfo.PlayerNick = recv["nick"];
                client.PlayerInfo.PlayerEmail = recv["email"];
            }
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
                //GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //client.DisconnectByReason(DisconnectReason.InvalidPlayer);
            }
            if (!Enum.TryParse(queryResult["userstatus"].ToString(), out currentUserStatus))
            {
                msg = "Invalid player data! Please contact an administrator.";
                reason = DisconnectReason.InvalidPlayer;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                //client.DisconnectByReason(DisconnectReason.InvalidPlayer);
                //return;
            }
            // Check the status of the account.
            // If the single profile is banned, the account or the player status
            if (currentPlayerStatus == PlayerStatus.Banned)
            {
                msg = "Your profile has been permanently suspended.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Your profile has been permanently suspended.");
                //client.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (currentUserStatus == UserStatus.Created)
            {
                msg = "Your account is not verified. Please check your email inbox and verify the account.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;

                //GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Your account is not verified. Please check your email inbox and verify the account.");
                //client.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }
            if (currentUserStatus == UserStatus.Banned)
            {
                msg = "Your account has been permanently suspended.";
                reason = DisconnectReason.PlayerIsBanned;
                return GPErrorCode.LoginBadUniquenick;
                //GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Your account has been permanently suspended.");
                //client.DisconnectByReason(DisconnectReason.PlayerIsBanned);
                //return;
            }

            msg = "No error";
            reason = DisconnectReason.NormalLogout;
            return GPErrorCode.NoError;
        }

        private static string SetPlayerInfo(GPCMClient client, Dictionary<string, object> queryResult, Dictionary<string, string> recv)
        {
            client.PlayerInfo.PlayerId = uint.Parse(queryResult["profileid"].ToString());
            client.PlayerInfo.PasswordHash = queryResult["password"].ToString().ToLowerInvariant();
            client.PlayerInfo.PlayerCountryCode = queryResult["countrycode"].ToString();
            client.PlayerInfo.PlayerFirstName = queryResult["firstname"].ToString();
            client.PlayerInfo.PlayerLastName = queryResult["lastname"].ToString();
            client.PlayerInfo.PlayerICQ = int.Parse(queryResult["icq"].ToString());
            client.PlayerInfo.PlayerHomepage = queryResult["homepage"].ToString();
            client.PlayerInfo.PlayerZIPCode = queryResult["zipcode"].ToString();
            client.PlayerInfo.PlayerLocation = queryResult["location"].ToString();
            client.PlayerInfo.PlayerAim = queryResult["aim"].ToString();
            client.PlayerInfo.PlayerOwnership = int.Parse(queryResult["ownership1"].ToString());
            client.PlayerInfo.PlayerOccupation = int.Parse(queryResult["occupationid"].ToString());
            client.PlayerInfo.PlayerIndustryID = int.Parse(queryResult["industryid"].ToString());
            client.PlayerInfo.PlayerIncomeID = int.Parse(queryResult["incomeid"].ToString());
            client.PlayerInfo.PlayerMarried = int.Parse(queryResult["marriedid"].ToString());
            client.PlayerInfo.PlayerChildCount = int.Parse(queryResult["childcount"].ToString());
            client.PlayerInfo.PlayerConnectionType = int.Parse(queryResult["connectiontype"].ToString());
            client.PlayerInfo.PlayerPicture = int.Parse(queryResult["picture"].ToString());
            client.PlayerInfo.PlayerInterests = int.Parse(queryResult["interests1"].ToString());
            client.PlayerInfo.PlayerBirthday = ushort.Parse(queryResult["birthday"].ToString());
            client.PlayerInfo.PlayerBirthmonth = ushort.Parse(queryResult["birthmonth"].ToString());
            client.PlayerInfo.PlayerBirthyear = ushort.Parse(queryResult["birthyear"].ToString());

            PlayerSexType playerSexType;
            if (!Enum.TryParse(queryResult["sex"].ToString().ToUpper(), out playerSexType))
                client.PlayerInfo.PlayerSex = PlayerSexType.PAT;
            else
                client.PlayerInfo.PlayerSex = playerSexType;

            client.PlayerInfo.PlayerLatitude = float.Parse(queryResult["latitude"].ToString());
            client.PlayerInfo.PlayerLongitude = float.Parse(queryResult["longitude"].ToString());

            PublicMasks mask;
            if (!Enum.TryParse(queryResult["publicmask"].ToString(), out mask))
                client.PlayerInfo.PlayerPublicMask = PublicMasks.MASK_ALL;
            else
                client.PlayerInfo.PlayerPublicMask = mask;
            string challengeData;

            if (client.PlayerInfo.PlayerUniqueNick.Length > 0)
            {
                client.PlayerInfo.PlayerEmail = queryResult["email"].ToString();
                client.PlayerInfo.PlayerNick = queryResult["nick"].ToString();
                challengeData = client.PlayerInfo.PlayerUniqueNick;
                return challengeData;
            }
            else if (client.PlayerInfo.PlayerAuthToken.Length > 0)
            {
                client.PlayerInfo.PlayerEmail = queryResult["email"].ToString();
                client.PlayerInfo.PlayerNick = queryResult["nick"].ToString();
                client.PlayerInfo.PlayerUniqueNick = queryResult["uniquenick"].ToString();
                challengeData = client.PlayerInfo.PlayerAuthToken;
                return challengeData;
            }
            else
            {
                client.PlayerInfo.PlayerUniqueNick = queryResult["uniquenick"].ToString();
                challengeData = recv["user"];
                return challengeData;
            }

        }

        /// <summary>
        /// Generates an MD5 hash, which is used to verify the clients login information
        /// </summary>
        /// <param name="challenge1">First challenge key</param>
        /// <param name="challenge2">Second challenge key</param>
        /// <param name="userdata">The user data to append to the proof</param>
        /// <param name="partnerid">The partnerid to append</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the client sends,
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
