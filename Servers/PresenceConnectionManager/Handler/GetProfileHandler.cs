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
            GPErrorCode error;
            if (!recv.ContainsKey("profileid"))
            {
                error = GPErrorCode.Parse;
                return;
            }
            if (!recv.ContainsKey("sesskey"))
            {
                error = GPErrorCode.Parse;
                return;
            }
            else
            { error = GPErrorCode.NoError; }

            if (error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, error, "Parsing error.");
            }

            var result = GetProfileQuery.GetProfileInfo(Convert.ToUInt32(recv["profileid"]));
            if (result == null)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.GetProfileBadProfile, "Unable to get profile information.");
                return;
            }

            string sendingBuffer = @"\pi\profileid\" + recv["profileid"] + @"\mp\4";

            sendingBuffer = string.Format(sendingBuffer + @"\nick\{0}\uniquenick\{1}\id\{2}", result["nick"], result["uniquenick"], recv["profileid"]);

            if (result["email"].ToString().Length > 0)
                sendingBuffer += @"\email\" + result["email"];

            if (result["lastname"].ToString().Length > 0)
                sendingBuffer += @"\lastname\" + result["lastname"];

            if (result["firstname"].ToString().Length > 0)
                sendingBuffer += @"\firstname\" + result["firstname"];

            if (Convert.ToInt32(result["icquin"]) != 0)
                sendingBuffer += @"\icquin\" + result["icquin"];

            if (result["homepage"].ToString().Length > 0)
                sendingBuffer += @"\homepage\" + result["homepage"];

            if (Convert.ToInt32(result["picture"]) != 0)
                sendingBuffer += @"\pic\" + result["picture"];

            if (result["aim"].ToString().Length > 0)
                sendingBuffer += @"\aim\" + result["aim"];

            if (Convert.ToInt32(result["occupationid"]) != 0)
                sendingBuffer += @"\occ\" + result["occupationid"];

            if (result["zipcode"].ToString().Length > 0)
                sendingBuffer += @"\zipcode\" + result["zipcode"];

            if (result["countrycode"].ToString().Length > 0)
                sendingBuffer += @"\countrycode\" + result["countrycode"];

            if (Convert.ToInt32(result["birthday"]) > 0 && Convert.ToInt32(result["birthmonth"]) > 0 && Convert.ToInt32(result["birthyear"]) > 0)
                sendingBuffer += @"\birthday\" + (uint)((Convert.ToInt32(result["birthday"]) << 24) | (Convert.ToInt32(result["birthmonth"]) << 16) | Convert.ToInt32(result["birthyear"]));

            if (Convert.ToInt32(result["location"]) > 0)
                sendingBuffer += @"\loc\" + result["location"];


            if ((PlayerSexType)result["sex"] != PlayerSexType.PAT)
            {
                if ((PlayerSexType)result["sex"] == PlayerSexType.FEMALE)
                    sendingBuffer += @"\sex\1";
                else if ((PlayerSexType)result["sex"] == PlayerSexType.MALE)
                    sendingBuffer += @"\sex\0";
            }

            float latitude;
            float.TryParse(result["latitude"].ToString(), out latitude);
            if (latitude != 0.0f)
                sendingBuffer += @"\lat\" + latitude;

            float longtitude;
            float.TryParse(result["latitude"].ToString(), out longtitude);
            if (longtitude != 0.0f)
                sendingBuffer += @"\lon\" + longtitude;

            if (Convert.ToInt32(result["incomeid"]) != 0)
                sendingBuffer += @"\inc\" + Convert.ToInt32(result["incomeid"]);

            if (Convert.ToInt32(result["industryid"]) != 0)
                sendingBuffer += @"\ind\" + Convert.ToInt32(result["industryid"]);

            if (Convert.ToInt32(result["marriedid"]) != 0)
                sendingBuffer += @"\mar\" + Convert.ToInt32(result["marriedid"]);

            if (Convert.ToInt32(result["childcount"]) != 0)
                sendingBuffer += @"\chc\" + Convert.ToInt32(result["childcount"]);

            if (Convert.ToInt32(result["interests1"]) != 0)
                sendingBuffer += @"\i1\" + Convert.ToInt32(result["interests1"]);

            if (Convert.ToInt32(result["ownership1"]) != 0)
                sendingBuffer += @"\o1\" + Convert.ToInt32(result["ownership1"]);

            if (Convert.ToInt32(result["connectiontype"]) != 0)
                sendingBuffer += @"\conn\" + Convert.ToInt32(result["connectiontype"]);

            // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
            string signature = GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex);
            sendingBuffer += @"\sig\" + signature + @"\final\";

            session.SendAsync(sendingBuffer);
        }
    }
}

//private static string SetPlayerInfo(GPCMSession session, Dictionary<string, object> queryResult, Dictionary<string, string> recv)
//{
//    session.PlayerInfo.PlayerId = uint.Parse(queryResult["profileid"].ToString());
//    session.PlayerInfo.PasswordHash = queryResult["password"].ToString().ToLowerInvariant();
//    session.PlayerInfo.PlayerCountryCode = queryResult["countrycode"].ToString();
//    session.PlayerInfo.PlayerFirstName = queryResult["firstname"].ToString();
//    session.PlayerInfo.PlayerLastName = queryResult["lastname"].ToString();
//    session.PlayerInfo.PlayerICQ = int.Parse(queryResult["icq"].ToString());
//    session.PlayerInfo.PlayerHomepage = queryResult["homepage"].ToString();
//    session.PlayerInfo.PlayerZIPCode = queryResult["zipcode"].ToString();
//    session.PlayerInfo.PlayerLocation = queryResult["location"].ToString();
//    session.PlayerInfo.PlayerAim = queryResult["aim"].ToString();
//    session.PlayerInfo.PlayerOwnership = int.Parse(queryResult["ownership1"].ToString());
//    session.PlayerInfo.PlayerOccupation = int.Parse(queryResult["occupationid"].ToString());
//    session.PlayerInfo.PlayerIndustryID = int.Parse(queryResult["industryid"].ToString());
//    session.PlayerInfo.PlayerIncomeID = int.Parse(queryResult["incomeid"].ToString());
//    session.PlayerInfo.PlayerMarried = int.Parse(queryResult["marriedid"].ToString());
//    session.PlayerInfo.PlayerChildCount = int.Parse(queryResult["childcount"].ToString());
//    session.PlayerInfo.PlayerConnectionType = int.Parse(queryResult["connectiontype"].ToString());
//    session.PlayerInfo.PlayerPicture = int.Parse(queryResult["picture"].ToString());
//    session.PlayerInfo.PlayerInterests = int.Parse(queryResult["interests1"].ToString());
//    session.PlayerInfo.PlayerBirthday = ushort.Parse(queryResult["birthday"].ToString());
//    session.PlayerInfo.PlayerBirthmonth = ushort.Parse(queryResult["birthmonth"].ToString());
//    session.PlayerInfo.PlayerBirthyear = ushort.Parse(queryResult["birthyear"].ToString());

//    PlayerSexType playerSexType;
//    if (!Enum.TryParse(queryResult["sex"].ToString().ToUpper(), out playerSexType))
//        session.PlayerInfo.PlayerSex = PlayerSexType.PAT;
//    else
//        session.PlayerInfo.PlayerSex = playerSexType;

//    session.PlayerInfo.PlayerLatitude = float.Parse(queryResult["latitude"].ToString());
//    session.PlayerInfo.PlayerLongitude = float.Parse(queryResult["longitude"].ToString());

//    PublicMasks mask;
//    if (!Enum.TryParse(queryResult["publicmask"].ToString(), out mask))
//        session.PlayerInfo.PlayerPublicMask = PublicMasks.MASK_ALL;
//    else
//        session.PlayerInfo.PlayerPublicMask = mask;

//}