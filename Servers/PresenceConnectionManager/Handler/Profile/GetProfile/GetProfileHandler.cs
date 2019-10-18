using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Profile.GetProfile.Query;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.GetProfile
{

    public class GetProfileHandler
    {
        static Dictionary<string, string> _recv;
        static GPErrorCode _error;
        /// <summary>
        /// Send others profile.
        /// </summary>
        public static void SendProfile(GPCMSession session, Dictionary<string, string> recv)
        {
            //TODO
            _recv = recv;
            // \getprofile\\sesskey\19150\profileid\2\id\2\final\
            //profileid is 

            IsContainAllKey();

            if (_error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _error, "Parsing error.");
                return;
            }

            var result = GetProfileQuery.GetProfileInfo(Convert.ToUInt32(_recv["profileid"]), Convert.ToUInt32(_recv["sesskey"]));

            if (result == null)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.GetProfileBadProfile, "Unable to get profile information.");
                return;
            }

            string sendingBuffer = @"\pi\\profileid\" + _recv["profileid"];

            if (result["nick"].ToString().Length > 0)
                sendingBuffer += @"\nick\" + result["nick"].ToString();

            if (result["uniquenick"].ToString().Length > 0)
                sendingBuffer += @"\uniquenick\" + result["uniquenick"].ToString();

            if (result["email"].ToString().Length > 0)
                sendingBuffer += @"\email\" + result["email"].ToString();

            if (result["firstname"].ToString().Length > 0)
                sendingBuffer += @"\firstname\" + result["firstname"].ToString();

            if (result["lastname"].ToString().Length > 0)
                sendingBuffer += @"\firstname\" + result["lastname"].ToString();

            if (result["icquin"].ToString().Length > 0)
                sendingBuffer += @"\icquin\" + Convert.ToUInt32(result["icquin"]);

            if (result["homepage"].ToString().Length > 0)
                sendingBuffer += @"\homepage\" + result["homepage"].ToString();

            if (result["zipcode"].ToString().Length > 0)
                sendingBuffer += @"\zipcode\" + result["zipcode"].ToString();

            if (result["countrycode"].ToString().Length > 0)
                sendingBuffer += @"\countrycode\" + result["countrycode"].ToString();


            float longtitude;
            if (result["longitude"].ToString().Length > 0)
            {
                float.TryParse(result["longitude"].ToString(), out longtitude);
                sendingBuffer += @"\lon\" + longtitude;
            }


            float latitude;
            if (result["latitude"].ToString().Length > 0)
            {
                float.TryParse(result["latitude"].ToString(), out latitude);
                sendingBuffer += @"\lat\" + latitude;
            }

            if (result["location"].ToString().Length > 0)
                sendingBuffer += @"\loc\" + result["location"].ToString();

            if (result["birthday"].ToString().Length > 0 && result["birthmonth"].ToString().Length > 0 && result["birthyear"].ToString().Length > 0)
            {
                uint birthday = (Convert.ToUInt32(result["birthday"]) << 24) | (Convert.ToUInt32(result["birthmonth"]) << 16) | Convert.ToUInt32(result["birthyear"]);
                sendingBuffer += @"\birthday\" + birthday;
            }

            PlayerSexType playerSexType;
            if (!Enum.TryParse(result["sex"].ToString().ToUpper(), out playerSexType))
                if (playerSexType == PlayerSexType.PAT)
                    sendingBuffer += @"\sex\2";
            if (playerSexType == PlayerSexType.FEMALE)
                sendingBuffer += @"\sex\1";
            if (playerSexType == PlayerSexType.MALE)
                sendingBuffer += @"\sex\0";


            PublicMasks mask;
            if (result["publicmask"].ToString().Length > 0)
                if (!Enum.TryParse(result["publicmask"].ToString(), out mask))
                    sendingBuffer += @"\publicmask\" + mask;

            if (result["aim"].ToString().Length > 0)
                sendingBuffer += @"\aim\" + result["aim"].ToString();

            if (result["picture"].ToString().Length > 0)
                sendingBuffer += @"\picture\" + Convert.ToUInt16(result["picture"]);

            if (result["occupationid"].ToString().Length > 0)
                sendingBuffer += @"\occ\" + Convert.ToUInt16(result["occupationid"]);
            if (result["industryid"].ToString().Length > 0)
                sendingBuffer += @"\ind\" + Convert.ToUInt16(result["industryid"]);
            if (result["incomeid"].ToString().Length > 0)
                sendingBuffer += @"\inc\" + Convert.ToUInt16(result["incomeid"]);


            if (result["marriedid"].ToString().Length > 0)
                sendingBuffer += @"\mar\" + Convert.ToUInt16(result["marriedid"]);
            if (result["childcount"].ToString().Length > 0)
                sendingBuffer += @"\chc\" + Convert.ToUInt16(result["childcount"]);

            if (result["interests1"].ToString().Length > 0)
                sendingBuffer += @"\i1\" + result["interests1"].ToString();
            if (result["ownership1"].ToString().Length > 0)
                sendingBuffer += @"\o1\" + result["ownership1"].ToString();
            if (result["connectiontype"].ToString().Length > 0)
                sendingBuffer += @"\conn\" + Convert.ToUInt16(result["connectiontype"]);

            // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
            string signature = GameSpyLib.Common.Random.GenerateRandomString(10, GameSpyLib.Common.Random.StringType.Hex);
            sendingBuffer += @"\sig\" + signature+@"\id\"+recv["id"]+@"\final\";


            session.SendAsync(sendingBuffer);
            session.PlayerInfo.BuddiesSent = true;
        }

        /// <summary>
        /// Send myself profile.
        /// </summary>
        /// <param name="session"></param>
        public static void SendProfile(GPCMSession session)
        {
            // \getprofile\\sesskey\19150\profileid\2\id\2\final\
            //profileid is 

            IsContainAllKey();

            if (_error != GPErrorCode.NoError)
            {
                GameSpyUtils.SendGPError(session, _error, "Parsing error.");
                return;
            }

            var result = GetProfileQuery.GetProfileInfo(Convert.ToUInt32(session.PlayerInfo.Profileid), session.PlayerInfo.SessionKey);

            if (result == null)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.GetProfileBadProfile, "Unable to get profile information.");
                return;
            }

            string sendingBuffer = @"\pi\profileid\" + _recv["profileid"];

            if (result["nick"].ToString().Length > 0)
                sendingBuffer += @"\nick\" + result["nick"].ToString();

            if (result["uniquenick"].ToString().Length > 0)
                sendingBuffer += @"\uniquenick\" + result["uniquenick"].ToString();

            if (result["email"].ToString().Length > 0)
                sendingBuffer += @"\email\" + result["email"].ToString();

            if (result["firstname"].ToString().Length > 0)
                sendingBuffer += @"\firstname\" + result["firstname"].ToString();

            if (result["lastname"].ToString().Length > 0)
                sendingBuffer += @"\firstname\" + result["lastname"].ToString();

            if (result["icquin"].ToString().Length > 0)
                sendingBuffer += @"\icquin\" + Convert.ToUInt32(result["icquin"]);

            if (result["homepage"].ToString().Length > 0)
                sendingBuffer += @"\homepage\" + result["homepage"].ToString();

            if (result["zipcode"].ToString().Length > 0)
                sendingBuffer += @"\zipcode\" + result["zipcode"].ToString();

            if (result["countrycode"].ToString().Length > 0)
                sendingBuffer += @"\countrycode\" + result["countrycode"].ToString();


            float longtitude;
            if (result["longtitude"].ToString().Length > 0)
            {
                float.TryParse(result["longtitude"].ToString(), out longtitude);
                sendingBuffer += @"\lon\" + longtitude;
            }


            float latitude;
            if (result["latitude"].ToString().Length > 0)
            {
                float.TryParse(result["latitude"].ToString(), out latitude);
                sendingBuffer += @"\lat\" + latitude;
            }

            if (result["location"].ToString().Length > 0)
                sendingBuffer += @"\loc\" + result["location"].ToString();

            if (result["birthday"].ToString().Length > 0 && result["birthmonth"].ToString().Length > 0 && result["birthyear"].ToString().Length > 0)
            {
                uint birthday = (Convert.ToUInt32(result["birthday"]) << 24) | (Convert.ToUInt32(result["birthmonth"]) << 16) | Convert.ToUInt32(result["birthyear"]);
                sendingBuffer += @"\birthday\" + birthday;
            }

            if (result["sex"].ToString().Length>0)
                if ((PlayerSexType)result["sex"] == PlayerSexType.PAT)
                    sendingBuffer += @"\sex\2";
            if ((PlayerSexType)result["sex"] == PlayerSexType.FEMALE)
                sendingBuffer += @"\sex\1";
            if ((PlayerSexType)result["sex"] == PlayerSexType.MALE)
                sendingBuffer += @"\sex\0";


            PublicMasks mask;
            if (result["publicmask"].ToString().Length > 0)
                if (!Enum.TryParse(result["publicmask"].ToString(), out mask))
                    sendingBuffer += @"\publicmask\" + mask;

            if (result["aim"].ToString().Length > 0)
                sendingBuffer += @"\aim\" + result["aim"].ToString();

            if (result["picture"].ToString().Length > 0)
                sendingBuffer += @"\picture\" + Convert.ToUInt32(result["picture"]);

            if (result["occupationid"].ToString().Length > 0)
                sendingBuffer += @"\occ\" + result["occupationid"].ToString();
            if (result["industryid"].ToString().Length > 0)
                sendingBuffer += @"\ind\" + result["industryid"].ToString();
            if (result["incomeid"].ToString().Length > 0)
                sendingBuffer += @"\inc\" + result["incomeid"].ToString();


            if (result["marriedid"].ToString().Length > 0)
                sendingBuffer += @"\mar\" + result["marriedid"].ToString();
            if (result["childcount"].ToString().Length > 0)
                sendingBuffer += @"\chc\" + result["childcount"].ToString();

            if (result["interests1"].ToString().Length > 0)
                sendingBuffer += @"\i1\" + result["interests1"].ToString();
            if (result["ownership1"].ToString().Length > 0)
                sendingBuffer += @"\o1\" + result["ownership1"].ToString();
            if (result["connectiontype"].ToString().Length > 0)
                sendingBuffer += @"\conn\" + result["connectiontype"].ToString();

            // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
            string signature = GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex);
            sendingBuffer += @"\sig\" + signature;


            session.SendAsync(sendingBuffer);
            session.PlayerInfo.BuddiesSent = true;
        }

        private static void IsContainAllKey()
        {
            if (!_recv.ContainsKey("profileid"))
            {
                _error = GPErrorCode.Parse;
                return;
            }
            if (!_recv.ContainsKey("sesskey"))
            {
                _error = GPErrorCode.Parse;
                return;
            }
            _error = GPErrorCode.NoError;
        }
    }


}
//if (result["email"].ToString().Length > 0)
//    sendingBuffer += @"\email\" + result["email"];

//if (result["lastname"].ToString().Length > 0)
//    sendingBuffer += @"\lastname\" + result["lastname"];

//if (result["firstname"].ToString().Length > 0)
//    sendingBuffer += @"\firstname\" + result["firstname"];

//if (Convert.ToInt32(result["icquin"]) != 0)
//    sendingBuffer += @"\icquin\" + result["icquin"];

//if (result["homepage"].ToString().Length > 0)
//    sendingBuffer += @"\homepage\" + result["homepage"];

//if (Convert.ToInt32(result["picture"]) != 0)
//    sendingBuffer += @"\pic\" + result["picture"];

//if (result["aim"].ToString().Length > 0)
//    sendingBuffer += @"\aim\" + result["aim"];

//if (Convert.ToInt32(result["occupationid"]) != 0)
//    sendingBuffer += @"\occ\" + result["occupationid"];

//if (result["zipcode"].ToString().Length > 0)
//    sendingBuffer += @"\zipcode\" + result["zipcode"];

//if (result["countrycode"].ToString().Length > 0)
//    sendingBuffer += @"\countrycode\" + result["countrycode"];

//if (Convert.ToInt32(result["birthday"]) > 0 && Convert.ToInt32(result["birthmonth"]) > 0 && Convert.ToInt32(result["birthyear"]) > 0)
//    sendingBuffer += @"\birthday\" + (uint)((Convert.ToInt32(result["birthday"]) << 24) | (Convert.ToInt32(result["birthmonth"]) << 16) | Convert.ToInt32(result["birthyear"]));

//if (result.ContainsKey("location"))
//    sendingBuffer += @"\loc\" + result["location"].ToString();


//if (result["sex"].ToString() != "PAT")
//{
//    if (result["sex"].ToString() == "FEMALE")
//        sendingBuffer += @"\sex\1";
//    else if (result["sex"].ToString() == "MALE")
//        sendingBuffer += @"\sex\0";
//}

//float latitude;
//float.TryParse(result["latitude"].ToString(), out latitude);
//if (latitude != 0.0f)
//    sendingBuffer += @"\lat\" + latitude;

//float longtitude;
//float.TryParse(result["latitude"].ToString(), out longtitude);
//if (longtitude != 0.0f)
//    sendingBuffer += @"\lon\" + longtitude;

//if (Convert.ToInt32(result["incomeid"]) != 0)
//    sendingBuffer += @"\inc\" + Convert.ToInt32(result["incomeid"]);

//if (Convert.ToInt32(result["industryid"]) != 0)
//    sendingBuffer += @"\ind\" + Convert.ToInt32(result["industryid"]);

//if (Convert.ToInt32(result["marriedid"]) != 0)
//    sendingBuffer += @"\mar\" + Convert.ToInt32(result["marriedid"]);

//if (Convert.ToInt32(result["childcount"]) != 0)
//    sendingBuffer += @"\chc\" + Convert.ToInt32(result["childcount"]);

//if (Convert.ToInt32(result["interests1"]) != 0)
//    sendingBuffer += @"\i1\" + Convert.ToInt32(result["interests1"]);

//if (Convert.ToInt32(result["ownership1"]) != 0)
//    sendingBuffer += @"\o1\" + Convert.ToInt32(result["ownership1"]);

//if (Convert.ToInt32(result["connectiontype"]) != 0)
//    sendingBuffer += @"\conn\" + Convert.ToInt32(result["connectiontype"]);

//// SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
//string signature = GameSpyLib.Common.Random.GenerateRandomString(33, GameSpyLib.Common.Random.StringType.Hex);
//sendingBuffer += @"\sig\" + signature + @"\final\";
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