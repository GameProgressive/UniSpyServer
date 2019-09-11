using GameSpyLib.Common;
using GameSpyLib.Extensions;
using GameSpyLib.Logging;
using RetroSpyServer.Application;
using RetroSpyServer.Servers.GPCM.Enumerator;
using RetroSpyServer.Servers.GPSP.Enumerators;
using System;
using System.Collections.Generic;

namespace RetroSpyServer.Servers.GPCM.Handler
{
   public  class LoginHandler
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
        public static void ProcessLogin(GPCMClient client,Dictionary<string, string> Recv, GPCMConnectionUpdate OnSuccessfulLogin, GPCMStatusChanged OnStatusChanged)
        {
            uint partnerID = 0;

            // Make sure we have all the required data to process this login
            if (!Recv.ContainsKey("challenge") || !Recv.ContainsKey("response"))
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "Invalid response received from the client!");
                client.Disconnect(DisconnectReason.InvalidLoginQuery);
                return;
            }

            // Parse the partnerid, required since it changes the challenge for Unique nick and User login
            if (Recv.ContainsKey("partnerid"))
            {
                if (!uint.TryParse(Recv["partnerid"], out partnerID))
                    partnerID = 0;
            }

            // Parse the 3 login types information
            if (Recv.ContainsKey("uniquenick"))
            {
                client.PlayerInfo.PlayerUniqueNick = Recv["uniquenick"];
            }
            else if (Recv.ContainsKey("authtoken"))
            {
                client.PlayerInfo.PlayerAuthToken = Recv["authtoken"];
            }
            else if (Recv.ContainsKey("user"))
            {
                // "User" is <nickname>@<email>
                string User = Recv["user"];
                int Pos = User.IndexOf('@');
                client.PlayerInfo.PlayerNick = User.Substring(0, Pos);
                client.PlayerInfo.PlayerEmail = User.Substring(Pos + 1);
            }

            // Dispose connection after use
            try
            {
                // Try and fetch the user from the database
                Dictionary<string, object> queryResult;

                try
                {
                    if (client.PlayerInfo.PlayerUniqueNick.Length > 0)
                        queryResult = GPCMHandler.DBQuery.GetUserFromUniqueNick(client.PlayerInfo.PlayerUniqueNick);
                    else if (client.PlayerInfo.PlayerAuthToken.Length > 0)
                    {
                        //TODO! Add the database entry
                        GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "AuthToken is not supported yet");
                        return;
                    }
                    else
                        queryResult = GPCMHandler.DBQuery.GetUserFromNickname(client.PlayerInfo.PlayerEmail, client.PlayerInfo.PlayerNick);
                }
                catch (Exception ex)
                {
                    LogWriter.Log.WriteException(ex);
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "This request cannot be processed because of a database error.");
                    return;
                }

                if (queryResult == null)
                {
                    if (client.PlayerInfo.PlayerUniqueNick.Length > 0)
                        GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "The uniquenick provided is incorrect!");
                    else
                        GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "The nick provided is incorrect!");

                    client.Disconnect(DisconnectReason.InvalidUsername);
                    return;
                }

                // Check if user is banned
                PlayerStatus currentPlayerStatus;
                UserStatus currentUserStatus;

                if (!Enum.TryParse(queryResult["profilestatus"].ToString(), out currentPlayerStatus))
                {
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                    client.Disconnect(DisconnectReason.InvalidPlayer);
                    return;
                }

                if (!Enum.TryParse(queryResult["userstatus"].ToString(), out currentUserStatus))
                {
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Invalid player data! Please contact an administrator.");
                    client.Disconnect(DisconnectReason.InvalidPlayer);
                    return;
                }

                // Check the status of the account.
                // If the single profile is banned, the account or the player status

                if (currentPlayerStatus == PlayerStatus.Banned)
                {
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Your profile has been permanently suspended.");
                    client.Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                if (currentUserStatus == UserStatus.Created)
                {
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Your account is not verified. Please check your email inbox and verify the account.");
                    client.Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                if (currentUserStatus == UserStatus.Banned)
                {
                    GameSpyUtils.SendGPError(client.Stream, GPErrorCode.LoginBadUniquenick, "Your account has been permanently suspended.");
                    client.Disconnect(DisconnectReason.PlayerIsBanned);
                    return;
                }

                // Set player variables
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

                string challengeData = "";

                if (client.PlayerInfo.PlayerUniqueNick.Length > 0)
                {
                    client.PlayerInfo.PlayerEmail = queryResult["email"].ToString();
                    client.PlayerInfo.PlayerNick = queryResult["nick"].ToString();
                    challengeData = client.PlayerInfo.PlayerUniqueNick;
                }
                else if (client.PlayerInfo.PlayerAuthToken.Length > 0)
                {
                    client.PlayerInfo.PlayerEmail = queryResult["email"].ToString();
                    client.PlayerInfo.PlayerNick = queryResult["nick"].ToString();
                    client.PlayerInfo.PlayerUniqueNick = queryResult["uniquenick"].ToString();
                    challengeData = client.PlayerInfo.PlayerAuthToken;
                }
                else
                {
                    client.PlayerInfo.PlayerUniqueNick = queryResult["uniquenick"].ToString();
                    challengeData = Recv["user"];
                }

                // Use the GenerateProof method to compare with the "response" value. This validates the given password
                if (Recv["response"] == GPCMHandler.GenerateProof(Recv["challenge"], client.ServerChallengeKey, challengeData, client.PlayerInfo.PlayerAuthToken.Length > 0 ? 0 : partnerID, client.PlayerInfo))
                {
                    // Create session key
                    client.SessionKey = Crc.ComputeChecksum(client.PlayerInfo.PlayerUniqueNick);

                    // Password is correct
                    client.Stream.SendAsync(
                        @"\lc\2\sesskey\{0}\proof\{1}\userid\{2}\profileid\{2}\uniquenick\{3}\lt\{4}__\id\1\final\",
                        client.SessionKey,
                        GPCMHandler.GenerateProof(client.ServerChallengeKey, Recv["challenge"], challengeData, client.PlayerInfo.PlayerAuthToken.Length > 0 ? 0 : partnerID, client.PlayerInfo), // Do this again, Params are reversed!
                        client.PlayerInfo.PlayerId,
                        client.PlayerInfo.PlayerNick,
                        GameSpyLib.Common.Random.GenerateRandomString(22, GameSpyLib.Common.Random.StringType.Hex) // Generate LT whatever that is (some sort of random string, 22 chars long)
                    );

                    // Log Incoming Connections
                    //LogWriter.Log.Write(LogLevel.Info, "{0,-8} [Login] {1} - {2} - {3}", client.Stream.ServerName, client.PlayerInfo.PlayerNick, client.PlayerInfo.PlayerId, RemoteEndPoint);
                    client.Stream.ToLog(LogLevel.Info, "Login", "Success", "{0} - {1} - {2}", client.PlayerInfo.PlayerNick, client.PlayerInfo.PlayerId, client.RemoteEndPoint);
                    // Update status last, and call success login
                    client.PlayerInfo.LoginStatus = LoginStatus.Completed;
                    client.PlayerInfo.PlayerStatus = PlayerStatus.Online;
                    client.PlayerInfo.PlayerStatusString = "Online";
                    client.PlayerInfo.PlayerStatusLocation = "";

                    client.CompletedLoginProcess = true;
                    OnSuccessfulLogin?.Invoke(client);
                    OnStatusChanged?.Invoke(client);
                    GPCMHandler.SendBuddies(client);
                }
                else
                {
                    // Log Incoming Connections
                    //LogWriter.Log.Write(LogLevel.Info, "{0,-8} [Login] Failed: {1} - {2} - {3}", client.Stream.ServerName, client.PlayerInfo.PlayerNick, client.PlayerInfo.PlayerId, RemoteEndPoint);
                    client.Stream.ToLog(LogLevel.Info, "Login", "Failed", "{0} - {1} - {2}", client.PlayerInfo.PlayerNick, client.PlayerInfo.PlayerId, client.RemoteEndPoint);
                    // Password is incorrect with database value
                    client.Stream.SendAsync(@"\error\\err\260\fatal\\errmsg\The password provided is incorrect.\id\1\final\");
                    client.Disconnect(DisconnectReason.InvalidPassword);
                }
            }
            catch (Exception ex)
            {
                LogWriter.Log.Write(ex.ToString(), LogLevel.Error);
                client.Disconnect(DisconnectReason.GeneralError);
                return;
            }
        }
    }

}
