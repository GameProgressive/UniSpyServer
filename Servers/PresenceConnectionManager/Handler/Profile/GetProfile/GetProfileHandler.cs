using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.GetProfile
{

    public class GetProfileHandler
    {
        static Dictionary<string, string> _recv;
        static GPErrorCode _error;
        static string _sendingBuffer;
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

            //if (result["icquin"].ToString().Length > 0)
            //    sendingBuffer += @"\icquin\" + Convert.ToUInt32(result["icquin"]);

            //if (result["homepage"].ToString().Length > 0)
            //    sendingBuffer += @"\homepage\" + result["homepage"].ToString();

            //if (result["zipcode"].ToString().Length > 0)
            //    sendingBuffer += @"\zipcode\" + result["zipcode"].ToString();

            //if (result["countrycode"].ToString().Length > 0)
            //    sendingBuffer += @"\countrycode\" + result["countrycode"].ToString();


            //float longtitude;
            //if (result["longitude"].ToString().Length > 0)
            //{
            //    float.TryParse(result["longitude"].ToString(), out longtitude);
            //    sendingBuffer += @"\lon\" + longtitude;
            //}


            //float latitude;
            //if (result["latitude"].ToString().Length > 0)
            //{
            //    float.TryParse(result["latitude"].ToString(), out latitude);
            //    sendingBuffer += @"\lat\" + latitude;
            //}

            //if (result["location"].ToString().Length > 0)
            //    sendingBuffer += @"\loc\" + result["location"].ToString();

            //if (result["birthday"].ToString().Length > 0 && result["birthmonth"].ToString().Length > 0 && result["birthyear"].ToString().Length > 0)
            //{
            //    uint birthday = (Convert.ToUInt32(result["birthday"]) << 24) | (Convert.ToUInt32(result["birthmonth"]) << 16) | Convert.ToUInt32(result["birthyear"]);
            //    sendingBuffer += @"\birthday\" + birthday;
            //}

            //PlayerSexType playerSexType;
            //if (!Enum.TryParse(result["sex"].ToString().ToUpper(), out playerSexType))
            //    if (playerSexType == PlayerSexType.PAT)
            //        sendingBuffer += @"\sex\2";
            //if (playerSexType == PlayerSexType.FEMALE)
            //    sendingBuffer += @"\sex\1";
            //if (playerSexType == PlayerSexType.MALE)
            //    sendingBuffer += @"\sex\0";


            //PublicMasks mask;
            //if (result["publicmask"].ToString().Length > 0)
            //    if (!Enum.TryParse(result["publicmask"].ToString(), out mask))
            //        sendingBuffer += @"\publicmask\" + mask;

            //if (result["aim"].ToString().Length > 0)
            //    sendingBuffer += @"\aim\" + result["aim"].ToString();

            //if (result["picture"].ToString().Length > 0)
            //    sendingBuffer += @"\picture\" + Convert.ToUInt16(result["picture"]);

            //if (result["occupationid"].ToString().Length > 0)
            //    sendingBuffer += @"\occ\" + Convert.ToUInt16(result["occupationid"]);
            //if (result["industryid"].ToString().Length > 0)
            //    sendingBuffer += @"\ind\" + Convert.ToUInt16(result["industryid"]);
            //if (result["incomeid"].ToString().Length > 0)
            //    sendingBuffer += @"\inc\" + Convert.ToUInt16(result["incomeid"]);


            //if (result["marriedid"].ToString().Length > 0)
            //    sendingBuffer += @"\mar\" + Convert.ToUInt16(result["marriedid"]);
            //if (result["childcount"].ToString().Length > 0)
            //    sendingBuffer += @"\chc\" + Convert.ToUInt16(result["childcount"]);

            //if (result["interests1"].ToString().Length > 0)
            //    sendingBuffer += @"\i1\" + result["interests1"].ToString();
            //if (result["ownership1"].ToString().Length > 0)
            //    sendingBuffer += @"\o1\" + result["ownership1"].ToString();
            //if (result["connectiontype"].ToString().Length > 0)
            //    sendingBuffer += @"\conn\" + Convert.ToUInt16(result["connectiontype"]);

            // SUPER NOTE: Please check the Signature of the PID, otherwise when it will be compared with other peers, it will break everything (See gpiPeer.c @ peerSig)
            string signature = GameSpyLib.Common.GameSpyRandom.GenerateRandomString(10, GameSpyLib.Common.GameSpyRandom.StringType.Hex);
            sendingBuffer += @"\sig\" + signature+@"\id\"+recv["id"]+@"\final\";


            session.SendAsync(sendingBuffer);
            session.PlayerInfo.BuddiesSent = true;
        }

        private void ConstructResponseString(Dictionary<string,object> result)
        {
            _sendingBuffer = @"\pi\\profileid\" + _recv["profileid"];
            if (result["nick"].ToString().Length > 0)
                _sendingBuffer += @"\nick\" + result["nick"].ToString();

            if (result["uniquenick"].ToString().Length > 0)
                _sendingBuffer += @"\uniquenick\" + result["uniquenick"].ToString();

            if (result["email"].ToString().Length > 0)
                _sendingBuffer += @"\email\" + result["email"].ToString();

            if (result["firstname"].ToString().Length > 0)
                _sendingBuffer += @"\firstname\" + result["firstname"].ToString();

            if (result["lastname"].ToString().Length > 0)
                _sendingBuffer += @"\firstname\" + result["lastname"].ToString();

            if (result["icquin"].ToString().Length > 0)
                _sendingBuffer += @"\icquin\" + Convert.ToUInt32(result["icquin"]);

            if (result["homepage"].ToString().Length > 0)
                _sendingBuffer += @"\homepage\" + result["homepage"].ToString();

            if (result["zipcode"].ToString().Length > 0)
                _sendingBuffer += @"\zipcode\" + result["zipcode"].ToString();

            if (result["countrycode"].ToString().Length > 0)
                _sendingBuffer += @"\countrycode\" + result["countrycode"].ToString();


            float longtitude;
            if (result["longitude"].ToString().Length > 0)
            {
                float.TryParse(result["longitude"].ToString(), out longtitude);
                _sendingBuffer += @"\lon\" + longtitude;
            }


            float latitude;
            if (result["latitude"].ToString().Length > 0)
            {
                float.TryParse(result["latitude"].ToString(), out latitude);
                _sendingBuffer += @"\lat\" + latitude;
            }

            if (result["location"].ToString().Length > 0)
                _sendingBuffer += @"\loc\" + result["location"].ToString();

            if (result["birthday"].ToString().Length > 0 && result["birthmonth"].ToString().Length > 0 && result["birthyear"].ToString().Length > 0)
            {
                uint birthday = (Convert.ToUInt32(result["birthday"]) << 24) | (Convert.ToUInt32(result["birthmonth"]) << 16) | Convert.ToUInt32(result["birthyear"]);
                _sendingBuffer += @"\birthday\" + birthday;
            }

            PlayerSexType playerSexType;
            if (!Enum.TryParse(result["sex"].ToString().ToUpper(), out playerSexType))
                if (playerSexType == PlayerSexType.Pat)
                    _sendingBuffer += @"\sex\2";
            if (playerSexType == PlayerSexType.Female)
                _sendingBuffer += @"\sex\1";
            if (playerSexType == PlayerSexType.Male)
                _sendingBuffer += @"\sex\0";


            PublicMasks mask;
            if (result["publicmask"].ToString().Length > 0)
                if (!Enum.TryParse(result["publicmask"].ToString(), out mask))
                    _sendingBuffer += @"\publicmask\" + mask;

            if (result["aim"].ToString().Length > 0)
                _sendingBuffer += @"\aim\" + result["aim"].ToString();

            if (result["picture"].ToString().Length > 0)
                _sendingBuffer += @"\picture\" + Convert.ToUInt16(result["picture"]);

            if (result["occupationid"].ToString().Length > 0)
                _sendingBuffer += @"\occ\" + Convert.ToUInt16(result["occupationid"]);
            if (result["industryid"].ToString().Length > 0)
                _sendingBuffer += @"\ind\" + Convert.ToUInt16(result["industryid"]);
            if (result["incomeid"].ToString().Length > 0)
                _sendingBuffer += @"\inc\" + Convert.ToUInt16(result["incomeid"]);


            if (result["marriedid"].ToString().Length > 0)
                _sendingBuffer += @"\mar\" + Convert.ToUInt16(result["marriedid"]);
            if (result["childcount"].ToString().Length > 0)
                _sendingBuffer += @"\chc\" + Convert.ToUInt16(result["childcount"]);

            if (result["interests1"].ToString().Length > 0)
                _sendingBuffer += @"\i1\" + result["interests1"].ToString();
            if (result["ownership1"].ToString().Length > 0)
                _sendingBuffer += @"\o1\" + result["ownership1"].ToString();
            if (result["connectiontype"].ToString().Length > 0)
                _sendingBuffer += @"\conn\" + Convert.ToUInt16(result["connectiontype"]);
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
