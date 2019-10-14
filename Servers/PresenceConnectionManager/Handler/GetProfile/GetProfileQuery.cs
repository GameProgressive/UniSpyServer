using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Structure;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.GetProfile
{
    public class GetProfileQuery
    {
        /// <summary>
        /// The user profile you searched must be accurate, 
        /// because it contains uniquenick, 
        /// we know that there may be multiple uniquenicks with different namespaceid productid partnerid under a nick,
        /// so we determine the unique namespaceid productid partnerid based on the incoming sesskey to determine uniquenick
        /// </summary>
        /// <param name="profileid"></param>
        /// <returns></returns>
        public static Dictionary<string, object> GetProfileInfo(uint profileid, uint sessionkey)
        {
            var result = GPCMServer.DB.Query(
                @"SELECT 
                profiles.nick,
                namespace.uniquenick,
                users.email,
                profiles.firstname,
                profiles.lastname,
                profiles.icquin, 
                profiles.homepage,
                profiles.zipcode,  
                profiles.countrycode,  
                profiles.longitude,
                profiles.latitude, 
                profiles.location,
                profiles.birthday, 
                profiles.birthmonth,
                profiles.birthyear, 
                profiles.sex, 
                profiles.publicmask, 
                profiles.aim,
                profiles.picture, 
                profiles.occupationid, 
                profiles.industryid,
                profiles.incomeid,
                profiles.marriedid,
                profiles.childcount, 
                profiles.interests1,
                profiles.ownership1,
                profiles.connectiontype 
                FROM profiles 
                LEFT JOIN namespace ON namespace.profileid = profiles.profileid
                LEFT JOIN users ON users.userid = profiles.userid
                WHERE profiles.profileid=@P0 and sesskey=@P1", profileid, sessionkey);
            return (result.Count == 0) ? null : result[0];

            //var data = Rows[0];

            //GPCMPlayerInfo playerInfo = new GPCMPlayerInfo();

            //uint uintTemp;
            //int intTemp;
            //float floatTemp;

            //if (!uint.TryParse(data["publicmask"].ToString(), out uintTemp))
            //    playerInfo.PlayerPublicMask = PublicMasks.MASK_NONE;
            //else
            //    playerInfo.PlayerPublicMask = (PublicMasks)uintTemp;

            //playerInfo.PlayerNick = data["nick"].ToString();
            //playerInfo.PlayerUniqueNick = data["uniquenick"].ToString();

            //if (playerInfo.PlayerPublicMask != PublicMasks.MASK_NONE)
            //{
            //    if (data["email"].ToString().Length > 0)
            //    {
            //        if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_EMAIL) > 0)
            //            playerInfo.PlayerEmail = data["email"].ToString();
            //    }

            //    if (data["lastname"].ToString().Length > 0)
            //        playerInfo.PlayerLastName = data["lastname"].ToString();

            //    if (data["firstname"].ToString().Length > 0)
            //        playerInfo.PlayerFirstName = data["firstname"].ToString();

            //    if (int.TryParse(data["icq"].ToString(), out intTemp))
            //        playerInfo.PlayerICQ = intTemp;


            //    if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_HOMEPAGE) > 0)
            //        playerInfo.PlayerEmail = data["email"].ToString();

            //    if (int.TryParse(data["picture"].ToString(), out intTemp))
            //        playerInfo.PlayerPicture = intTemp;

            //    if (data["aim"].ToString().Length > 0)
            //        playerInfo.PlayerAim = data["aim"].ToString();

            //    if (int.TryParse(data["occupationid"].ToString(), out intTemp))
            //        playerInfo.PlayerOccupation = intTemp;

            //    if (data["zipcode"].ToString().Length > 0)
            //    {
            //        if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_ZIPCODE) > 0)
            //            playerInfo.PlayerZIPCode = data["zipcode"].ToString();
            //    }

            //    if (data["countrycode"].ToString().Length > 0)
            //    {
            //        if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_COUNTRYCODE) > 0)
            //            playerInfo.PlayerCountryCode = data["countrycode"].ToString();
            //    }

            //    ushort bday, byear, bmonth;
            //    if (ushort.TryParse(data["birthday"].ToString(), out bday) && ushort.TryParse(data["birthmonth"].ToString(), out bmonth) && ushort.TryParse(data["birthyear"].ToString(), out byear))
            //    {
            //        if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_BIRTHDAY) > 0)
            //        {
            //            playerInfo.PlayerBirthday = bday;
            //            playerInfo.PlayerBirthmonth = bmonth;
            //            playerInfo.PlayerBirthyear = byear;
            //        }
            //    }

            //    if (data["location"].ToString().Length > 0)
            //        playerInfo.PlayerLocation = data["location"].ToString();

            //    if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_SEX) > 0)
            //    {
            //        if (!Enum.TryParse(data["sex"].ToString(), out playerInfo.PlayerSex))
            //            playerInfo.PlayerSex = PlayerSexType.PAT;
            //    }

            //    if (float.TryParse(data["latitude"].ToString(), out floatTemp))
            //        playerInfo.PlayerLatitude = floatTemp;

            //    if (float.TryParse(data["longitude"].ToString(), out floatTemp))
            //        playerInfo.PlayerLongitude = floatTemp;

            //    if (int.TryParse(data["incomeid"].ToString(), out intTemp))
            //        playerInfo.PlayerIncomeID = intTemp;

            //    if (int.TryParse(data["industryid"].ToString(), out intTemp))
            //        playerInfo.PlayerIndustryID = intTemp;

            //    if (int.TryParse(data["marriedid"].ToString(), out intTemp))
            //        playerInfo.PlayerMarried = intTemp;

            //    if (int.TryParse(data["childcount"].ToString(), out intTemp))
            //        playerInfo.PlayerChildCount = intTemp;

            //    if (int.TryParse(data["interests1"].ToString(), out intTemp))
            //        playerInfo.PlayerInterests = intTemp;

            //    if (int.TryParse(data["ownership1"].ToString(), out intTemp))
            //        playerInfo.PlayerOwnership = intTemp;

            //    if (int.TryParse(data["connectiontype"].ToString(), out intTemp))
            //        playerInfo.PlayerConnectionType = intTemp;
            //}

            //return playerInfo;
        }
    }
}
