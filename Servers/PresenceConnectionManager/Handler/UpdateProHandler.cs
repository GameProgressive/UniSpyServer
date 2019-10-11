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
        public static void UpdateUser(GPCMSession session, Dictionary<string, string> recv)
        {
            // Set clients country code
            if (!recv.ContainsKey("sesskey"))
                return;

            ushort ssk;
            if (!ushort.TryParse(recv["sesskey"], out ssk))
                return;

            if (ssk != session.PlayerInfo.SessionKey)
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
                    if (session.PlayerInfo.PlayerPublicMask != mask)
                    {
                        query += ", publicmask=@P0";
                        session.PlayerInfo.PlayerPublicMask = mask;
                        passData[0] = mask;
                    }
                }
            }

            if (recv.ContainsKey("firstname"))
            {
              
                if (recv["firstname"] != session.PlayerInfo.PlayerFirstName)
                {
                    query += ", firstname=@P1";
                    session.PlayerInfo.PlayerFirstName = recv["firstname"];
                    passData[1] = session.PlayerInfo.PlayerFirstName;
                }
            }

            if (recv.ContainsKey("lastname"))
            {
                if (recv["lastname"] != session.PlayerInfo.PlayerLastName)
                {
                    query += ", lastname=@P2";
                    session.PlayerInfo.PlayerFirstName = recv["lastname"];
                    passData[2] = session.PlayerInfo.PlayerLastName;
                }
            }

            if (recv.ContainsKey("icquin"))
            {
                int icq = 0;

                if (int.TryParse(recv["icquin"], out icq))
                {
                    if (icq != session.PlayerInfo.PlayerICQ)
                    {
                        query += "icq=@P3 ";
                        session.PlayerInfo.PlayerICQ = icq;
                        passData[3] = icq;
                    }
                }
            }

            if (recv.ContainsKey("homepage"))
            {
                if (recv["homepage"] != session.PlayerInfo.PlayerHomepage)
                {
                    query += ", homepage=@P4";
                    session.PlayerInfo.PlayerHomepage = recv["homepage"];
                    passData[4] = session.PlayerInfo.PlayerHomepage;
                }
            }

            if (recv.ContainsKey("zipcode"))
            {
                if (recv["zipcode"] != session.PlayerInfo.PlayerZIPCode)
                {
                    query += ", zipcode=@P5";
                    session.PlayerInfo.PlayerZIPCode = recv["zipcode"];
                    passData[5] = session.PlayerInfo.PlayerZIPCode;
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
                        if (session.PlayerInfo.PlayerBirthday != d)
                        {
                            query += ", birthday=@P6";
                            passData[6] = d;
                            session.PlayerInfo.PlayerBirthday = d;
                        }

                        if (session.PlayerInfo.PlayerBirthmonth != m)
                        {
                            query += ", birthmonth=@P19";
                            passData[19] = m;
                            session.PlayerInfo.PlayerBirthmonth = m;
                        }

                        if (session.PlayerInfo.PlayerBirthyear != y)
                        {
                            query += ", birthyear=@P20";
                            passData[20] = y;
                            session.PlayerInfo.PlayerBirthyear = y;
                        }
                    }
                }

                if (recv.ContainsKey("countrycode"))
                {
                    if (recv["countrycode"] != session.PlayerInfo.PlayerCountryCode)
                    {
                        query += ", countrycode=@P7";
                        session.PlayerInfo.PlayerCountryCode = recv["zipcode"];
                        passData[7] = session.PlayerInfo.PlayerCountryCode;
                    }
                }                
            }


            if (recv.ContainsKey("sex"))
            {
                PlayerSexType sex;
                if (Enum.TryParse(recv["sex"], out sex))
                {
                    if (session.PlayerInfo.PlayerSex != sex)
                    {
                        query += "sex=@P8";
                        session.PlayerInfo.PlayerSex = sex;

                        if (session.PlayerInfo.PlayerSex == PlayerSexType.MALE)
                            passData[8] = "MALE";
                        else if (session.PlayerInfo.PlayerSex == PlayerSexType.FEMALE)
                            passData[8] = "FEMALE";
                        else
                            passData[8] = "PAT";
                    }
                }
            }

            if (recv.ContainsKey("aim"))
            {
                if (recv["aim"] != session.PlayerInfo.PlayerAim)
                {
                    query += ", aim=@P9";
                    session.PlayerInfo.PlayerAim = recv["aim"];
                    passData[9] = session.PlayerInfo.PlayerAim;
                }
            }

            if (recv.ContainsKey("pic"))
            {
                int pic = 0;

                if (int.TryParse(recv["pic"], out pic))
                {
                    if (pic != session.PlayerInfo.PlayerPicture)
                    {
                        query += ", picture=@P10";
                        session.PlayerInfo.PlayerPicture = pic;
                        passData[10] = pic;
                    }
                }
            }

            if (recv.ContainsKey("occ"))
            {
                int occ = 0;

                if (int.TryParse(recv["occ"], out occ))
                {
                    if (occ != session.PlayerInfo.PlayerOccupation)
                    {
                        query += ", occupationid=@P11";
                        session.PlayerInfo.PlayerOccupation = occ;
                        passData[11] = occ;
                    }
                }
            }

            if (recv.ContainsKey("ind"))
            {
                int ind = 0;

                if (int.TryParse(recv["ind"], out ind))
                {
                    if (ind != session.PlayerInfo.PlayerIndustryID)
                    {
                        query += ", industryid=@P12";
                        session.PlayerInfo.PlayerIndustryID = ind;
                        passData[12] = ind;
                    }
                }
            }

            if (recv.ContainsKey("inc"))
            {
                int inc = 0;

                if (int.TryParse(recv["inc"], out inc))
                {
                    if (inc != session.PlayerInfo.PlayerIncomeID)
                    {
                        query += ", industryid=@P13";
                        session.PlayerInfo.PlayerIncomeID = inc;
                        passData[13] = inc;
                    }
                }
            }

            if (recv.ContainsKey("mar"))
            {
                int mar = 0;

                if (int.TryParse(recv["mar"], out mar))
                {
                    if (mar != session.PlayerInfo.PlayerMarried)
                    {
                        query += ", marriedid=@P14";
                        session.PlayerInfo.PlayerMarried = mar;
                        passData[14] = mar;
                    }
                }
            }

            if (recv.ContainsKey("chc"))
            {
                int chc = 0;

                if (int.TryParse(recv["chc"], out chc))
                {
                    if (chc != session.PlayerInfo.PlayerChildCount)
                    {
                        query += ", childcount=@P15";
                        session.PlayerInfo.PlayerChildCount = chc;
                        passData[15] = chc;
                    }
                }
            }

            if (recv.ContainsKey("i1"))
            {
                int i1 = 0;

                if (int.TryParse(recv["i1"], out i1))
                {
                    if (i1 != session.PlayerInfo.PlayerInterests)
                    {
                        query += ", interests1=@P16";
                        session.PlayerInfo.PlayerInterests = i1;
                        passData[16] = i1;
                    }
                }
            }

            if (recv.ContainsKey("nick"))
            {
                if (recv["nick"] != session.PlayerInfo.PlayerNick)
                {
                    query += ", nick=@P17";
                    session.PlayerInfo.PlayerNick = recv["nick"];
                    passData[17] = session.PlayerInfo.PlayerNick;
                }
            }

            if (recv.ContainsKey("uniquenick"))
            {
                if (recv["uniquenick"] != session.PlayerInfo.PlayerUniqueNick)
                {
                    query += ", uniquenick=@P18";
                    session.PlayerInfo.PlayerHomepage = recv["uniquenick"];
                    passData[18] = session.PlayerInfo.PlayerUniqueNick;
                }
            }

            if (query == "UPDATE profiles SET")
                return;

            query = query.Replace("SET,", "SET");

            passData[21] = session.PlayerInfo.PlayerId;
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
