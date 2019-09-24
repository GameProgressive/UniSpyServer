using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceConnectionManager.DatabaseQuery;
using PresenceConnectionManager.Enumerator;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler
{
    public class UpdateProHandler
    {
        /// <summary>
        /// Updates profiles
        /// </summary>
        /// <param name="recv">Array of information sent by the server</param>
        public static void UpdateUser(GPCMClient client, Dictionary<string, string> recv)
        {
            // Set clients country code
            if (!recv.ContainsKey("sesskey"))
                return;

            ushort ssk;
            if (!ushort.TryParse(recv["sesskey"], out ssk))
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
            

            if (recv.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (Enum.TryParse(recv["publicmask"], out mask))
                {
                    if (client.PlayerInfo.PlayerPublicMask != mask)
                    {
                        query += ", publicmask=@P0";
                        client.PlayerInfo.PlayerPublicMask = mask;
                        passData[0] = mask;
                    }
                }
            }

            if (recv.ContainsKey("firstname"))
            {
              
                if (recv["firstname"] != client.PlayerInfo.PlayerFirstName)
                {
                    query += ", firstname=@P1";
                    client.PlayerInfo.PlayerFirstName = recv["firstname"];
                    passData[1] = client.PlayerInfo.PlayerFirstName;
                }
            }

            if (recv.ContainsKey("lastname"))
            {
                if (recv["lastname"] != client.PlayerInfo.PlayerLastName)
                {
                    query += ", lastname=@P2";
                    client.PlayerInfo.PlayerFirstName = recv["lastname"];
                    passData[2] = client.PlayerInfo.PlayerLastName;
                }
            }

            if (recv.ContainsKey("icquin"))
            {
                int icq = 0;

                if (int.TryParse(recv["icquin"], out icq))
                {
                    if (icq != client.PlayerInfo.PlayerICQ)
                    {
                        query += "icq=@P3 ";
                        client.PlayerInfo.PlayerICQ = icq;
                        passData[3] = icq;
                    }
                }
            }

            if (recv.ContainsKey("homepage"))
            {
                if (recv["homepage"] != client.PlayerInfo.PlayerHomepage)
                {
                    query += ", homepage=@P4";
                    client.PlayerInfo.PlayerHomepage = recv["homepage"];
                    passData[4] = client.PlayerInfo.PlayerHomepage;
                }
            }

            if (recv.ContainsKey("zipcode"))
            {
                if (recv["zipcode"] != client.PlayerInfo.PlayerZIPCode)
                {
                    query += ", zipcode=@P5";
                    client.PlayerInfo.PlayerZIPCode = recv["zipcode"];
                    passData[5] = client.PlayerInfo.PlayerZIPCode;
                }
            }

            //if (recv.ContainsKey("countrycode"))
            //{
            //    if (recv["countrycode"] != client.PlayerInfo.PlayerCountryCode)
            //    {
            //        query += ", countrycode=@P6";
            //        client.PlayerInfo.PlayerCountryCode = recv["zipcode"];
            //        passData[6] = client.PlayerInfo.PlayerCountryCode;
            //    }
            //}

            if (recv.ContainsKey("birthday"))
            {
                int date;
                if (int.TryParse(recv["birthday"], out date))
                {
                    ushort d = (ushort)((date >> 24) & 0xFF);
                    ushort m = (ushort)((date >> 16) & 0xFF);
                    ushort y = (ushort)(date & 0xFFFF);

                    if (GameSpyUtils.IsValidDate(d, m, y))
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

                if (recv.ContainsKey("countrycode"))
                {
                    if (recv["countrycode"] != client.PlayerInfo.PlayerCountryCode)
                    {
                        query += ", countrycode=@P7";
                        client.PlayerInfo.PlayerCountryCode = recv["zipcode"];
                        passData[7] = client.PlayerInfo.PlayerCountryCode;
                    }
                }                
            }


            if (recv.ContainsKey("sex"))
            {
                PlayerSexType sex;
                if (Enum.TryParse(recv["sex"], out sex))
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

            if (recv.ContainsKey("aim"))
            {
                if (recv["aim"] != client.PlayerInfo.PlayerAim)
                {
                    query += ", aim=@P9";
                    client.PlayerInfo.PlayerAim = recv["aim"];
                    passData[9] = client.PlayerInfo.PlayerAim;
                }
            }

            if (recv.ContainsKey("pic"))
            {
                int pic = 0;

                if (int.TryParse(recv["pic"], out pic))
                {
                    if (pic != client.PlayerInfo.PlayerPicture)
                    {
                        query += ", picture=@P10";
                        client.PlayerInfo.PlayerPicture = pic;
                        passData[10] = pic;
                    }
                }
            }

            if (recv.ContainsKey("occ"))
            {
                int occ = 0;

                if (int.TryParse(recv["occ"], out occ))
                {
                    if (occ != client.PlayerInfo.PlayerOccupation)
                    {
                        query += ", occupationid=@P11";
                        client.PlayerInfo.PlayerOccupation = occ;
                        passData[11] = occ;
                    }
                }
            }

            if (recv.ContainsKey("ind"))
            {
                int ind = 0;

                if (int.TryParse(recv["ind"], out ind))
                {
                    if (ind != client.PlayerInfo.PlayerIndustryID)
                    {
                        query += ", industryid=@P12";
                        client.PlayerInfo.PlayerIndustryID = ind;
                        passData[12] = ind;
                    }
                }
            }

            if (recv.ContainsKey("inc"))
            {
                int inc = 0;

                if (int.TryParse(recv["inc"], out inc))
                {
                    if (inc != client.PlayerInfo.PlayerIncomeID)
                    {
                        query += ", industryid=@P13";
                        client.PlayerInfo.PlayerIncomeID = inc;
                        passData[13] = inc;
                    }
                }
            }

            if (recv.ContainsKey("mar"))
            {
                int mar = 0;

                if (int.TryParse(recv["mar"], out mar))
                {
                    if (mar != client.PlayerInfo.PlayerMarried)
                    {
                        query += ", marriedid=@P14";
                        client.PlayerInfo.PlayerMarried = mar;
                        passData[14] = mar;
                    }
                }
            }

            if (recv.ContainsKey("chc"))
            {
                int chc = 0;

                if (int.TryParse(recv["chc"], out chc))
                {
                    if (chc != client.PlayerInfo.PlayerChildCount)
                    {
                        query += ", childcount=@P15";
                        client.PlayerInfo.PlayerChildCount = chc;
                        passData[15] = chc;
                    }
                }
            }

            if (recv.ContainsKey("i1"))
            {
                int i1 = 0;

                if (int.TryParse(recv["i1"], out i1))
                {
                    if (i1 != client.PlayerInfo.PlayerInterests)
                    {
                        query += ", interests1=@P16";
                        client.PlayerInfo.PlayerInterests = i1;
                        passData[16] = i1;
                    }
                }
            }

            if (recv.ContainsKey("nick"))
            {
                if (recv["nick"] != client.PlayerInfo.PlayerNick)
                {
                    query += ", nick=@P17";
                    client.PlayerInfo.PlayerNick = recv["nick"];
                    passData[17] = client.PlayerInfo.PlayerNick;
                }
            }

            if (recv.ContainsKey("uniquenick"))
            {
                if (recv["uniquenick"] != client.PlayerInfo.PlayerUniqueNick)
                {
                    query += ", uniquenick=@P18";
                    client.PlayerInfo.PlayerHomepage = recv["uniquenick"];
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
                UpdateProQuery.UpdateUserInfo(query, passData);
            }
            catch (Exception e)
            {
                LogWriter.Log.WriteException(e);
            }
        }
    }
}
