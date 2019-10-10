using GameSpyLib.Common;
using PresenceConnectionManager.DatabaseQuery;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler
{
    public class GetProfileHandler
    {
        /// <summary>
        /// This method is called when the client requests for the Account profile
        /// </summary>
        public static void SendProfile(GPCMSession session, Dictionary<string, string> recv)
        {
            //TODO

            // \getprofile\\sesskey\19150\profileid\2\id\2\final\
            //profileid is 

            if (!recv.ContainsKey("profileid"))
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            uint targetPID, messID, sesskey;
            if (!uint.TryParse(recv["profileid"], out targetPID))
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            if (!uint.TryParse(recv["id"], out messID))
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }
            if (!uint.TryParse(recv["sesskey"], out sesskey))
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "There was an error parsing an incoming request.");
                return;
            }

            string datatoSend = @"\pi\profileid\" + targetPID + @"\mp\4";

            // If the client want to access the public information
            // of another client
            if (targetPID != session.PlayerInfo.PlayerId)
            {
                GPCMPlayerInfo playerInfo = GetProfileQuery.GetProfileInfo(targetPID);
                if (playerInfo == null)
                {
                    GameSpyUtils.SendGPError(session, 4, "Unable to get profile information.");
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
                                                                session.PlayerInfo.PlayerNick,
                                                                session.PlayerInfo.PlayerUniqueNick,
                                                                session.PlayerInfo.PlayerEmail,
                                                                /*(ProfileSent ? "5" : "2")*/ messID,
                                                                session.PlayerInfo.PlayerPublicMask
                                                                );

                if (session.PlayerInfo.PlayerLastName.Length > 0)
                    datatoSend += @"\lastname\" + session.PlayerInfo.PlayerLastName;

                if (session.PlayerInfo.PlayerFirstName.Length > 0)
                    datatoSend += @"\firstname\" + session.PlayerInfo.PlayerFirstName;

                if (session.PlayerInfo.PlayerICQ != 0)
                    datatoSend += @"\icquin\" + session.PlayerInfo.PlayerICQ;

                if (session.PlayerInfo.PlayerHomepage.Length > 0)
                    datatoSend += @"\homepage\" + session.PlayerInfo.PlayerHomepage;

                if (session.PlayerInfo.PlayerPicture != 0)
                    datatoSend += @"\pic\" + session.PlayerInfo.PlayerPicture;

                if (session.PlayerInfo.PlayerAim.Length > 0)
                    datatoSend += @"\aim\" + session.PlayerInfo.PlayerAim;

                if (session.PlayerInfo.PlayerOccupation != 0)
                    datatoSend += @"\occ\" + session.PlayerInfo.PlayerOccupation;

                if (session.PlayerInfo.PlayerZIPCode.Length > 0)
                    datatoSend += @"\zipcode\" + session.PlayerInfo.PlayerZIPCode;

                if (session.PlayerInfo.PlayerCountryCode.Length > 0)
                    datatoSend += @"\countrycode\" + session.PlayerInfo.PlayerCountryCode;

                if (session.PlayerInfo.PlayerBirthday > 0 && session.PlayerInfo.PlayerBirthmonth > 0 && session.PlayerInfo.PlayerBirthyear > 0)
                    datatoSend += @"\birthday\" + (uint)((session.PlayerInfo.PlayerBirthday << 24) | (session.PlayerInfo.PlayerBirthmonth << 16) | session.PlayerInfo.PlayerBirthyear);

                if (session.PlayerInfo.PlayerLocation.Length > 0)
                    datatoSend += @"\loc\" + session.PlayerInfo.PlayerLocation;

                if (session.PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                    datatoSend += @"\sex\1";
                else if (session.PlayerInfo.PlayerSex == PlayerSexType.MALE)
                    datatoSend += @"\sex\0";

                if (session.PlayerInfo.PlayerLatitude != 0.0f)
                    datatoSend += @"\lat\" + session.PlayerInfo.PlayerLatitude;

                if (session.PlayerInfo.PlayerLongitude != 0.0f)
                    datatoSend += @"\lon\" + session.PlayerInfo.PlayerLongitude;

                if (session.PlayerInfo.PlayerIncomeID != 0)
                    datatoSend += @"\inc\" + session.PlayerInfo.PlayerIncomeID;

                if (session.PlayerInfo.PlayerIndustryID != 0)
                    datatoSend += @"\ind\" + session.PlayerInfo.PlayerIndustryID;

                if (session.PlayerInfo.PlayerMarried != 0)
                    datatoSend += @"\mar\" + session.PlayerInfo.PlayerMarried;

                if (session.PlayerInfo.PlayerChildCount != 0)
                    datatoSend += @"\chc\" + session.PlayerInfo.PlayerChildCount;

                if (session.PlayerInfo.PlayerInterests != 0)
                    datatoSend += @"\i1\" + session.PlayerInfo.PlayerInterests;

                if (session.PlayerInfo.PlayerOwnership != 0)
                    datatoSend += @"\o1\" + session.PlayerInfo.PlayerOwnership;

                if (session.PlayerInfo.PlayerConnectionType != 0)
                    datatoSend += @"\conn\" + session.PlayerInfo.PlayerConnectionType;

                // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
                datatoSend += @"\sig\" + GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex) + @"\final\";

                // Set that we SendAsync the profile initially
                if (!session.ProfileSent) session.ProfileSent = true;
            }

            session.SendAsync(datatoSend);
        }
    }
}
