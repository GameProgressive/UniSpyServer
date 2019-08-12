using GameSpyLib.Common;
using RetroSpyServer.Servers.GPCM.Enumerator;
using RetroSpyServer.Servers.GPCM.Structures;
using RetroSpyServer.Servers.GPSP.Enumerators;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPCM.Handler
{
    public class GetProfileHandler
    {
        /// <summary>
        /// This method is called when the client requests for the Account profile
        /// </summary>
        public static void SendProfile(GPCMClient client, Dictionary<string, string> dict)
        {
            if (!dict.ContainsKey("profileid"))
            {
                GamespyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            uint targetPID, messID;
            if (!uint.TryParse(dict["profileid"], out targetPID))
            {
                GamespyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            if (!uint.TryParse(dict["id"], out messID))
            {
                GamespyUtils.SendGPError(client.Stream, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            string datatoSend = @"\pi\\profileid\" + targetPID + @"\mp\4";

            // If the client want to access the public information
            // of another client
            if (targetPID != client.PlayerInfo.PlayerId)
            {
                GPCMPlayerInfo playerInfo = GPCMHandler.DBQuery.GetProfileInfo(targetPID);
                if (playerInfo == null)
                {
                    GamespyUtils.SendGPError(client.Stream, 4, "Unable to get profile information.");
                    return;
                }

                datatoSend = string.Format(datatoSend + @"\nick\{0}\uniquenick\{1}\id\{2}", playerInfo.PlayerNick, playerInfo.PlayerUniqueNick, messID);

                if (playerInfo.PlayerEmail.Length > 0)
                    datatoSend += @"\email\" + playerInfo.PlayerEmail;

                if (playerInfo.PlayerLastName.Length > 0)
                    datatoSend += @"\lastname\" + playerInfo.PlayerLastName;

                if (playerInfo.PlayerFirstName.Length > 0)
                    datatoSend += @"\firstname\" + playerInfo.PlayerFirstName;

                if (playerInfo.PlayerICQ != 0)
                    datatoSend += @"\icquin\" + playerInfo.PlayerICQ;

                if (playerInfo.PlayerHomepage.Length > 0)
                    datatoSend += @"\homepage\" + playerInfo.PlayerHomepage;

                if (playerInfo.PlayerPicture != 0)
                    datatoSend += @"\pic\" + playerInfo.PlayerPicture;

                if (playerInfo.PlayerAim.Length > 0)
                    datatoSend += @"\aim\" + playerInfo.PlayerAim;

                if (playerInfo.PlayerOccupation != 0)
                    datatoSend += @"\occ\" + playerInfo.PlayerOccupation;

                if (playerInfo.PlayerZIPCode.Length > 0)
                    datatoSend += @"\zipcode\" + playerInfo.PlayerZIPCode;

                if (playerInfo.PlayerCountryCode.Length > 0)
                    datatoSend += @"\countrycode\" + playerInfo.PlayerCountryCode;

                if (playerInfo.PlayerBirthday > 0 && playerInfo.PlayerBirthmonth > 0 && playerInfo.PlayerBirthyear > 0)
                    datatoSend += @"\birthday\" + (uint)((playerInfo.PlayerBirthday << 24) | (playerInfo.PlayerBirthmonth << 16) | playerInfo.PlayerBirthyear);

                if (playerInfo.PlayerLocation.Length > 0)
                    datatoSend += @"\loc\" + playerInfo.PlayerLocation;

                if (playerInfo.PlayerSex != PlayerSexType.PAT)
                {
                    if (playerInfo.PlayerSex == PlayerSexType.FEMALE)
                        datatoSend += @"\sex\1";
                    else if (playerInfo.PlayerSex == PlayerSexType.MALE)
                        datatoSend += @"\sex\0";
                }

                if (playerInfo.PlayerLatitude != 0.0f)
                    datatoSend += @"\lat\" + playerInfo.PlayerLatitude;

                if (playerInfo.PlayerLongitude != 0.0f)
                    datatoSend += @"\lon\" + playerInfo.PlayerLongitude;

                if (playerInfo.PlayerIncomeID != 0)
                    datatoSend += @"\inc\" + playerInfo.PlayerIncomeID;

                if (playerInfo.PlayerIndustryID != 0)
                    datatoSend += @"\ind\" + playerInfo.PlayerIndustryID;

                if (playerInfo.PlayerMarried != 0)
                    datatoSend += @"\mar\" + playerInfo.PlayerMarried;

                if (playerInfo.PlayerChildCount != 0)
                    datatoSend += @"\chc\" + playerInfo.PlayerChildCount;

                if (playerInfo.PlayerInterests != 0)
                    datatoSend += @"\i1\" + playerInfo.PlayerInterests;

                if (playerInfo.PlayerOwnership != 0)
                    datatoSend += @"\o1\" + playerInfo.PlayerOwnership;

                if (playerInfo.PlayerConnectionType != 0)
                    datatoSend += @"\conn\" + playerInfo.PlayerConnectionType;

                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                datatoSend += @"\sig\" + GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex) + @"\final\";
            }
            else
            {
                // Since this is our profile, we have to see ALL informations that we can edit. This means that we don't need to check the public masks for sending
                // the data

                datatoSend = string.Format(datatoSend + @"\nick\{0}\uniquenick\{1}\email\{2}\id\{3}\pmask\{4}",
                                                                client.PlayerInfo.PlayerNick,
                                                                client.PlayerInfo.PlayerUniqueNick,
                                                                client.PlayerInfo.PlayerEmail,
                                                                /*(ProfileSent ? "5" : "2")*/ messID,
                                                                client.PlayerInfo.PlayerPublicMask
                                                                );

                if (client.PlayerInfo.PlayerLastName.Length > 0)
                    datatoSend += @"\lastname\" + client.PlayerInfo.PlayerLastName;

                if (client.PlayerInfo.PlayerFirstName.Length > 0)
                    datatoSend += @"\firstname\" + client.PlayerInfo.PlayerFirstName;

                if (client.PlayerInfo.PlayerICQ != 0)
                    datatoSend += @"\icquin\" + client.PlayerInfo.PlayerICQ;

                if (client.PlayerInfo.PlayerHomepage.Length > 0)
                    datatoSend += @"\homepage\" + client.PlayerInfo.PlayerHomepage;

                if (client.PlayerInfo.PlayerPicture != 0)
                    datatoSend += @"\pic\" + client.PlayerInfo.PlayerPicture;

                if (client.PlayerInfo.PlayerAim.Length > 0)
                    datatoSend += @"\aim\" + client.PlayerInfo.PlayerAim;

                if (client.PlayerInfo.PlayerOccupation != 0)
                    datatoSend += @"\occ\" + client.PlayerInfo.PlayerOccupation;

                if (client.PlayerInfo.PlayerZIPCode.Length > 0)
                    datatoSend += @"\zipcode\" + client.PlayerInfo.PlayerZIPCode;

                if (client.PlayerInfo.PlayerCountryCode.Length > 0)
                    datatoSend += @"\countrycode\" + client.PlayerInfo.PlayerCountryCode;

                if (client.PlayerInfo.PlayerBirthday > 0 && client.PlayerInfo.PlayerBirthmonth > 0 && client.PlayerInfo.PlayerBirthyear > 0)
                    datatoSend += @"\birthday\" + (uint)((client.PlayerInfo.PlayerBirthday << 24) | (client.PlayerInfo.PlayerBirthmonth << 16) | client.PlayerInfo.PlayerBirthyear);

                if (client.PlayerInfo.PlayerLocation.Length > 0)
                    datatoSend += @"\loc\" + client.PlayerInfo.PlayerLocation;

                if (client.PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                    datatoSend += @"\sex\1";
                else if (client.PlayerInfo.PlayerSex == PlayerSexType.MALE)
                    datatoSend += @"\sex\0";

                if (client.PlayerInfo.PlayerLatitude != 0.0f)
                    datatoSend += @"\lat\" + client.PlayerInfo.PlayerLatitude;

                if (client.PlayerInfo.PlayerLongitude != 0.0f)
                    datatoSend += @"\lon\" + client.PlayerInfo.PlayerLongitude;

                if (client.PlayerInfo.PlayerIncomeID != 0)
                    datatoSend += @"\inc\" + client.PlayerInfo.PlayerIncomeID;

                if (client.PlayerInfo.PlayerIndustryID != 0)
                    datatoSend += @"\ind\" + client.PlayerInfo.PlayerIndustryID;

                if (client.PlayerInfo.PlayerMarried != 0)
                    datatoSend += @"\mar\" + client.PlayerInfo.PlayerMarried;

                if (client.PlayerInfo.PlayerChildCount != 0)
                    datatoSend += @"\chc\" + client.PlayerInfo.PlayerChildCount;

                if (client.PlayerInfo.PlayerInterests != 0)
                    datatoSend += @"\i1\" + client.PlayerInfo.PlayerInterests;

                if (client.PlayerInfo.PlayerOwnership != 0)
                    datatoSend += @"\o1\" + client.PlayerInfo.PlayerOwnership;

                if (client.PlayerInfo.PlayerConnectionType != 0)
                    datatoSend += @"\conn\" + client.PlayerInfo.PlayerConnectionType;

                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                datatoSend += @"\sig\" + GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex) + @"\final\";

                // Set that we send the profile initially
                if (!client.ProfileSent) client.ProfileSent = true;
            }

            client.Stream.SendAsync(datatoSend);
        }
    }
}
