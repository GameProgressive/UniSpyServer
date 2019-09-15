using System.Collections.Generic;
using GameSpyLib.Database;
using GameSpyLib.Extensions;
using PresenceConnectionManager.Structures;
using PresenceConnectionManager.Enumerator;
using System;

namespace PresenceConnectionManager
{
    public class GPCMDBQuery : DBQueryBase
    {

        /// <summary>
        /// Use DBQueryBase Constructor to create database connection for us
        /// </summary>
        /// <param name="dbdriver"></param>
        public GPCMDBQuery(DatabaseDriver dbdriver) : base(dbdriver)
        {
        }


        
        protected Dictionary<string, object> GetUserDataReal(string AppendFirst, string SecondAppend, string _P0, string _P1)
        {
            var Rows = Query("SELECT profiles.profileid, profiles.firstname, profiles.lastname, profiles.publicmask, profiles.latitude, profiles.longitude, " +
                "profiles.aim, profiles.picture, profiles.occupationid, profiles.incomeid, profiles.industryid, profiles.marriedid, profiles.childcount, profiles.interests1, " +
                @"profiles.ownership1, profiles.connectiontype, profiles.sex, profiles.zipcode, profiles.countrycode, profiles.homepage, profiles.birthday, profiles.birthmonth, " +
                @"profiles.birthyear, profiles.location, profiles.icq, profiles.profilestatus, users.password, users.userstatus " + AppendFirst +
                " FROM profiles INNER JOIN users ON profiles.userid = users.userid WHERE " + SecondAppend, _P0, _P1);
            return (Rows.Count == 0) ? null : Rows[0];
        }

        /// <summary>
        /// The user profile you searched must be accurate, 
        /// because it contains uniquenick, 
        /// we know that there may be multiple uniquenicks with different namespaceid productid partnerid under a nick,
        /// so we determine the unique namespaceid productid partnerid based on the incoming sesskey to determine uniquenick
        /// </summary>
        /// <param name="profileid"></param>
        /// <returns></returns>
        public GPCMPlayerInfo GetProfileInfo(uint profileid)
        {
            //var Rows = Query("SELECT profiles.firstname, profiles.lastname, profiles.publicmask, profiles.latitude, profiles.longitude, " +
            //    "profiles.aim, profiles.picture, profiles.occupationid, profiles.incomeid, profiles.industryid, profiles.marriedid, profiles.childcount, profiles.interests1, " +
            //    @"profiles.ownership1, profiles.connectiontype, profiles.sex, profiles.zipcode, profiles.countrycode, profiles.homepage, profiles.birthday, profiles.birthmonth, " +
            //    @"profiles.birthyear, profiles.location, profiles.icq, profiles.profilestatus, profiles.nick, namespace.uniquenick, users.email FROM profiles " +
            //    @"INNER JOIN users ON users.userid = profiles.userid INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE profiles.profileid=@P0", profileid);
            var Rows= Query("SELECT * FROM profiles " +
                @"INNER JOIN users ON users.userid = profiles.userid "+
                @"INNER JOIN namespace ON namespace.profileid = profiles.profileid WHERE profiles.profileid=@P0", profileid);
            if (Rows.Count == 0)
                return null;

            var data = Rows[0];

            GPCMPlayerInfo playerInfo = new GPCMPlayerInfo();

            uint uintTemp;
            int intTemp;
            float floatTemp;

            if (!uint.TryParse(data["publicmask"].ToString(), out uintTemp))
                playerInfo.PlayerPublicMask = PublicMasks.MASK_NONE;
            else
                playerInfo.PlayerPublicMask = (PublicMasks)uintTemp;

            playerInfo.PlayerNick = data["nick"].ToString();
            playerInfo.PlayerUniqueNick = data["uniquenick"].ToString();

            if (playerInfo.PlayerPublicMask != PublicMasks.MASK_NONE)
            {
                if (data["email"].ToString().Length > 0)
                {
                    if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_EMAIL) > 0)
                        playerInfo.PlayerEmail = data["email"].ToString();
                }

                if (data["lastname"].ToString().Length > 0)
                    playerInfo.PlayerLastName = data["lastname"].ToString();

                if (data["firstname"].ToString().Length > 0)
                    playerInfo.PlayerFirstName = data["firstname"].ToString();

                if (int.TryParse(data["icq"].ToString(), out intTemp))
                    playerInfo.PlayerICQ = intTemp;


                if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_HOMEPAGE) > 0)
                    playerInfo.PlayerEmail = data["email"].ToString();

                if (int.TryParse(data["picture"].ToString(), out intTemp))
                    playerInfo.PlayerPicture = intTemp;

                if (data["aim"].ToString().Length > 0)
                    playerInfo.PlayerAim = data["aim"].ToString();

                if (int.TryParse(data["occupationid"].ToString(), out intTemp))
                    playerInfo.PlayerOccupation = intTemp;

                if (data["zipcode"].ToString().Length > 0)
                {
                    if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_ZIPCODE) > 0)
                        playerInfo.PlayerZIPCode = data["zipcode"].ToString();
                }

                if (data["countrycode"].ToString().Length > 0)
                {
                    if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_COUNTRYCODE) > 0)
                        playerInfo.PlayerCountryCode = data["countrycode"].ToString();
                }

                ushort bday, byear, bmonth;
                if (ushort.TryParse(data["birthday"].ToString(), out bday) && ushort.TryParse(data["birthmonth"].ToString(), out bmonth) && ushort.TryParse(data["birthyear"].ToString(), out byear))
                {
                    if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_BIRTHDAY) > 0)
                    {
                        playerInfo.PlayerBirthday = bday;
                        playerInfo.PlayerBirthmonth = bmonth;
                        playerInfo.PlayerBirthyear = byear;
                    }
                }

                if (data["location"].ToString().Length > 0)
                    playerInfo.PlayerLocation = data["location"].ToString();

                if ((playerInfo.PlayerPublicMask & PublicMasks.MASK_SEX) > 0)
                {
                    if (!Enum.TryParse(data["sex"].ToString(), out playerInfo.PlayerSex))
                        playerInfo.PlayerSex = PlayerSexType.PAT;
                }

                if (float.TryParse(data["latitude"].ToString(), out floatTemp))
                    playerInfo.PlayerLatitude = floatTemp;

                if (float.TryParse(data["longitude"].ToString(), out floatTemp))
                    playerInfo.PlayerLongitude = floatTemp;

                if (int.TryParse(data["incomeid"].ToString(), out intTemp))
                    playerInfo.PlayerIncomeID = intTemp;

                if (int.TryParse(data["industryid"].ToString(), out intTemp))
                    playerInfo.PlayerIndustryID = intTemp;

                if (int.TryParse(data["marriedid"].ToString(), out intTemp))
                    playerInfo.PlayerMarried = intTemp;

                if (int.TryParse(data["childcount"].ToString(), out intTemp))
                    playerInfo.PlayerChildCount = intTemp;

                if (int.TryParse(data["interests1"].ToString(), out intTemp))
                    playerInfo.PlayerInterests = intTemp;

                if (int.TryParse(data["ownership1"].ToString(), out intTemp))
                    playerInfo.PlayerOwnership = intTemp;

                if (int.TryParse(data["connectiontype"].ToString(), out intTemp))
                    playerInfo.PlayerConnectionType = intTemp;
            }

            return playerInfo;
        }

        public bool IsUniquenickExist(GPCMPlayerInfo player)
        {
            //first we check if this guy exists.
            List<Dictionary<string, object>> temp = Query(@"SELECT namespace.profileid FROM namespace INNER JOIN profiles ON" +
               @" profiles.profileid = namespace.profileid INNER JOIN users ON profiles.userid=users.userid" +
               @" WHERE users.email = @P0 AND users.password = @P1 AND namespace.uniquenick = @P2", player.PlayerEmail, player.PasswordHash, player.PlayerUniqueNick);
            if (temp.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Dictionary<string, object>> GetUserFromUniqueNick(Dictionary<string, string> dict)
        {
            return Query(@"SELECT profiles.profileid, profiles.nick, profiles.firstname, profiles.lastname, profiles.publicmask, profiles.latitude,"
             + @"profiles.longitude,profiles.aim, profiles.picture, profiles.occupationid, profiles.incomeid, profiles.industryid,"
             + @" profiles.marriedid, profiles.childcount, profiles.interests1,profiles.ownership1, profiles.connectiontype, profiles.sex, "
             + @"profiles.zipcode, profiles.countrycode, profiles.homepage, profiles.birthday, profiles.birthmonth ,profiles.birthyear, "
             + @"profiles.location, profiles.icq, profiles.profilestatus, users.email, users.password, users.userstatus "
             + @"FROM profiles INNER JOIN users ON profiles.userid = users.userid INNER JOIN namespace ON profiles.profileid = namespace.profileid  "
             + @"WHERE namespace.uniquenick = @P0 AND namespace.productid = @P1 AND namespace.partnerid = @P2 AND namespace.gamename = @P3"
                , dict["uniquenick"],dict["productid"],dict["partnerid"],dict["gamename"]);
            
        }

        public Dictionary<string, object> GetUserFromNickname(Dictionary<string, string> dict)
        {
            var queryResult = Query(@"SELECT profiles.profileid, profiles.firstname, profiles.lastname, profiles.publicmask, profiles.latitude,profiles.longitude,"
                + @"profiles.aim, profiles.picture, profiles.occupationid, profiles.incomeid, profiles.industryid,profiles.marriedid, profiles.childcount, "
                + @"profiles.interests1,profiles.ownership1, profiles.connectiontype, profiles.sex,profiles.zipcode, profiles.countrycode, profiles.homepage, "
                + @"profiles.birthday, profiles.birthmonth ,profiles.birthyear,profiles.location, profiles.icq, profiles.profilestatus, users.password, users.userstatus, namespace.uniquenick"
                + @" FROM profiles INNER JOIN users ON profiles.userid = users.userid INNER JOIN namespace ON profiles.profileid = namespace.profileid "
                + @"WHERE  namespace.partnerid = @P0  AND"
                + @" namespace.gamename = @P1 AND "
                + @"profiles.nick = @P2 AND"
                + @" users.email=@P3", dict["partnerid"], dict["gamename"], dict["nick"], dict["email"]);

            return (queryResult.Count == 0) ? null : queryResult[0];

            //
            //return GetUserDataReal(", profiles.uniquenick ", "profiles.nick=@P0 AND users.email=@P1", Nick, Email);
        }

        public bool UserExists(string Nick)
        {
            return (Query("SELECT profileid FROM profiles WHERE `nickname`=@P0", Nick).Count != 0);
        }

        /// <summary>
        /// Creates a new Gamespy Account
        /// </summary>
        /// <remarks>Used by the login server when a create account request is made</remarks>
        /// <param name="dbdriver">The database connection to use</param>
        /// <param name="Nick">The Account Name</param>
        /// <param name="Pass">The UN-HASHED Account Password</param>
        /// <param name="Email">The Account Email Address</param>
        /// <param name="Country">The Country Code for this Account</param>
        /// <param name="UniqueNick">The unique nickname for this Account</param>
        /// <returns>Returns the Player ID if sucessful, 0 otherwise</returns>
        public uint CreateUser(string Nick, string Pass, string Email, string Country, string UniqueNick)
        {
            Execute("INSERT INTO users(email, password) VALUES(@P0, @P1)", Email, StringExtensions.GetMD5Hash(Pass));
            var Rows = Query("SELECT userid FROM users WHERE email=@P0 and password=@P1", Email, Pass);
            if (Rows.Count < 1)
                return 0;

            Execute("INSERT INTO profiles(userid, nick, uniquenick, countrycode) VALUES(@P0, @P1, @P2, @P3)", Rows[0]["userid"], Nick, UniqueNick, Country);
            Rows = Query("SELECT profileid FROM profiles WHERE uniquenick=@P0", UniqueNick);
            if (Rows.Count < 1)
                return 0;

            return uint.Parse(Rows[0]["profileid"].ToString());
        }

        public void UpdateStatus(long timestamp, System.Net.IPAddress address, uint PlayerId, uint PlayerStatus)
        {
            Execute("UPDATE profiles SET profilestatus=@P3, lastip=@P0, lastonline=@P1 WHERE profileid=@P2", address, timestamp, PlayerId, PlayerStatus);
        }
        /// <summary>
        /// update sessionkey in database, when we are going to get someone's profile
        /// we first find uniquenick by search profileid and namespaceid and productid and partnerid  
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="sesskey"></param>
        /// <param name="player"></param>
        public void UpdateSessionKey(Dictionary<string, string> dict, ushort sesskey, GPCMPlayerInfo player)
        {
           Dictionary<string, object> temp = Query("SELECT id FROM namespace WHERE profileid = @P0 AND namespaceid = @P1 AND partnerid = @P2 AND productid=@P3 AND gamename = @P4",player.PlayerId,dict["namespaceid"],dict["partnerid"],dict["productid"],dict["gamename"])[0];
            uint id = Convert.ToUInt32(temp["id"]);
            Execute("UPDATE namespace SET sesskey = @P0 WHERE id = @P1 ", sesskey,id);
        }


        public void ResetStatusAndSessionKey()
        {
            Execute("UPDATE profiles SET status=0, sesskey = NULL");
        }

        public void UpdateUserInfo(string query, object[] passData)
        {
            Query(query, passData);
        }
    }
}
