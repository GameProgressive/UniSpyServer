using GameSpyLib.Common;
using GameSpyLib.Logging;
using RetroSpyServer.Servers.GPCM.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPCM.Handler
{
    public class UpdateProHandler
    {
        /// <summary>
        /// Updates the Users Country code when sent by the client
        /// </summary>
        /// <param name="dict">Array of information sent by the server</param>
        public static void UpdateUser(GPCMClient client, Dictionary<string, string> dict)
        {
            // Set clients country code
            if (!dict.ContainsKey("sesskey"))
                return;

            ushort ssk;
            if (!ushort.TryParse(dict["sesskey"], out ssk))
                return;

            if (ssk != client.SessionKey)
                return;

            string query = "UPDATE profiles SET";
            object[] passData = new object[22] {
                null, // publicmask : 0
                null, // firstname
                null, // lastname
                null, // icq
                null, // homepage
                null, // zipcode
                null, // countrycode
                null, // birthday
                null, // sex
                null, // aim
                null, // pic
                null, // occ
                null, // ind
                null, // inc
                null, // mar
                null, // chc
                null, // i1
                null, // nick
                null, // uniquenick
                null, // Bithmonth
                null, // Birthyear
                null  // ProfileID
            };

            if (dict.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (Enum.TryParse(dict["publicmask"], out mask))
                {
                    if (client.PlayerInfo.PlayerPublicMask != mask)
                    {
                        query += ", publicmask=@P0";
                        client.PlayerInfo.PlayerPublicMask = mask;
                        passData[0] = mask;
                    }
                }
            }

            if (dict.ContainsKey("firstname"))
            {
                if (dict["firstname"] != client.PlayerInfo.PlayerFirstName)
                {
                    query += ", firstname=@P1";
                    client.PlayerInfo.PlayerFirstName = dict["firstname"];
                    passData[1] = client.PlayerInfo.PlayerFirstName;
                }
            }

            if (dict.ContainsKey("lastname"))
            {
                if (dict["lastname"] != client.PlayerInfo.PlayerLastName)
                {
                    query += ", lastname=@P2";
                    client.PlayerInfo.PlayerFirstName = dict["lastname"];
                    passData[2] = client.PlayerInfo.PlayerLastName;
                }
            }

            if (dict.ContainsKey("icquin"))
            {
                int icq = 0;

                if (int.TryParse(dict["icquin"], out icq))
                {
                    if (icq != client.PlayerInfo.PlayerICQ)
                    {
                        query += "icq=@P3 ";
                        client.PlayerInfo.PlayerICQ = icq;
                        passData[3] = icq;
                    }
                }
            }

            if (dict.ContainsKey("homepage"))
            {
                if (dict["homepage"] != client.PlayerInfo.PlayerHomepage)
                {
                    query += ", homepage=@P4";
                    client.PlayerInfo.PlayerHomepage = dict["homepage"];
                    passData[4] = client.PlayerInfo.PlayerHomepage;
                }
            }

            if (dict.ContainsKey("zipcode"))
            {
                if (dict["zipcode"] != client.PlayerInfo.PlayerZIPCode)
                {
                    query += ", zipcode=@P5";
                    client.PlayerInfo.PlayerZIPCode = dict["zipcode"];
                    passData[5] = client.PlayerInfo.PlayerZIPCode;
                }
            }

            if (dict.ContainsKey("countrycode"))
            {
                if (dict["countrycode"] != client.PlayerInfo.PlayerCountryCode)
                {
                    query += ", countrycode=@P6";
                    client.PlayerInfo.PlayerCountryCode = dict["zipcode"];
                    passData[6] = client.PlayerInfo.PlayerCountryCode;
                }
            }

            if (dict.ContainsKey("birthday"))
            {
                int date;
                if (int.TryParse(dict["birthday"], out date))
                {
                    ushort d = (ushort)((date >> 24) & 0xFF);
                    ushort m = (ushort)((date >> 16) & 0xFF);
                    ushort y = (ushort)(date & 0xFFFF);

                    if (GamespyUtils.IsValidDate(d, m, y))
                    {
                        if (client.PlayerInfo.PlayerBirthday != d)
                        {
                            query += ", birthday=@P6";
                            passData[6] = d;
                            client.PlayerInfo.PlayerBirthday = d;
                        }

                        if (client.PlayerInfo.PlayerBirthmonth != m)
                        {
                            query += ", birthmonth=@P19";
                            passData[19] = m;
                            client.PlayerInfo.PlayerBirthmonth = m;
                        }

                        if (client.PlayerInfo.PlayerBirthyear != y)
                        {
                            query += ", birthyear=@P20";
                            passData[20] = y;
                            client.PlayerInfo.PlayerBirthyear = y;
                        }
                    }
                }


                if (dict["countrycode"] != client.PlayerInfo.PlayerCountryCode)
                {
                    query += ", countrycode=@P7";
                    client.PlayerInfo.PlayerCountryCode = dict["zipcode"];
                    passData[7] = client.PlayerInfo.PlayerCountryCode;
                }
            }


            if (dict.ContainsKey("sex"))
            {
                PlayerSexType sex;
                if (Enum.TryParse(dict["sex"], out sex))
                {
                    if (client.PlayerInfo.PlayerSex != sex)
                    {
                        query += "sex=@P8";
                        client.PlayerInfo.PlayerSex = sex;

                        if (client.PlayerInfo.PlayerSex == PlayerSexType.MALE)
                            passData[8] = "MALE";
                        else if (client.PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                            passData[8] = "FEMALE";
                        else
                            passData[8] = "PAT";
                    }
                }
            }

            if (dict.ContainsKey("aim"))
            {
                if (dict["aim"] != client.PlayerInfo.PlayerAim)
                {
                    query += ", aim=@P9";
                    client.PlayerInfo.PlayerAim = dict["aim"];
                    passData[9] = client.PlayerInfo.PlayerAim;
                }
            }

            if (dict.ContainsKey("pic"))
            {
                int pic = 0;

                if (int.TryParse(dict["pic"], out pic))
                {
                    if (pic != client.PlayerInfo.PlayerPicture)
                    {
                        query += ", picture=@P10";
                        client.PlayerInfo.PlayerPicture = pic;
                        passData[10] = pic;
                    }
                }
            }

            if (dict.ContainsKey("occ"))
            {
                int occ = 0;

                if (int.TryParse(dict["occ"], out occ))
                {
                    if (occ != client.PlayerInfo.PlayerOccupation)
                    {
                        query += ", occupationid=@P11";
                        client.PlayerInfo.PlayerOccupation = occ;
                        passData[11] = occ;
                    }
                }
            }

            if (dict.ContainsKey("ind"))
            {
                int ind = 0;

                if (int.TryParse(dict["ind"], out ind))
                {
                    if (ind != client.PlayerInfo.PlayerIndustryID)
                    {
                        query += ", industryid=@P12";
                        client.PlayerInfo.PlayerIndustryID = ind;
                        passData[12] = ind;
                    }
                }
            }

            if (dict.ContainsKey("inc"))
            {
                int inc = 0;

                if (int.TryParse(dict["inc"], out inc))
                {
                    if (inc != client.PlayerInfo.PlayerIncomeID)
                    {
                        query += ", industryid=@P13";
                        client.PlayerInfo.PlayerIncomeID = inc;
                        passData[13] = inc;
                    }
                }
            }

            if (dict.ContainsKey("mar"))
            {
                int mar = 0;

                if (int.TryParse(dict["mar"], out mar))
                {
                    if (mar != client.PlayerInfo.PlayerMarried)
                    {
                        query += ", marriedid=@P14";
                        client.PlayerInfo.PlayerMarried = mar;
                        passData[14] = mar;
                    }
                }
            }

            if (dict.ContainsKey("chc"))
            {
                int chc = 0;

                if (int.TryParse(dict["chc"], out chc))
                {
                    if (chc != client.PlayerInfo.PlayerChildCount)
                    {
                        query += ", childcount=@P15";
                        client.PlayerInfo.PlayerChildCount = chc;
                        passData[15] = chc;
                    }
                }
            }

            if (dict.ContainsKey("i1"))
            {
                int i1 = 0;

                if (int.TryParse(dict["i1"], out i1))
                {
                    if (i1 != client.PlayerInfo.PlayerInterests)
                    {
                        query += ", interests1=@P16";
                        client.PlayerInfo.PlayerInterests = i1;
                        passData[16] = i1;
                    }
                }
            }

            if (dict.ContainsKey("nick"))
            {
                if (dict["nick"] != client.PlayerInfo.PlayerNick)
                {
                    query += ", nick=@P17";
                    client.PlayerInfo.PlayerNick = dict["nick"];
                    passData[17] = client.PlayerInfo.PlayerNick;
                }
            }

            if (dict.ContainsKey("uniquenick"))
            {
                if (dict["uniquenick"] != client.PlayerInfo.PlayerUniqueNick)
                {
                    query += ", uniquenick=@P18";
                    client.PlayerInfo.PlayerHomepage = dict["uniquenick"];
                    passData[18] = client.PlayerInfo.PlayerUniqueNick;
                }
            }

            if (query == "UPDATE profiles SET")
                return;

            query = query.Replace("SET,", "SET");

            passData[21] = client.PlayerInfo.PlayerId;
            query += " WHERE `profileid`=@P21";

            try
            {
                GPCMHandler.DBQuery.UpdateUserInfo(query, passData);
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }
    }
}
