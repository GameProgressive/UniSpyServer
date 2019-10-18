using GameSpyLib.Common;
using PresenceConnectionManager.Enumerator;
using PresenceConnectionManager.Handler.Profile.UpdatePro.Query;
using System;
using System.Collections.Generic;

namespace PresenceConnectionManager.Handler.Profile.UpdatePro
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

            if (recv.ContainsKey("publicmask"))
            {
                PublicMasks mask;
                if (Enum.TryParse(recv["publicmask"], out mask))
                {
                    query += "publicmask=" + recv["publicmask"]+",";
                }
            }

            if (recv.ContainsKey("firstname"))
            {
                query += "firstname"+recv["firstname"] + ",";
                
            }

            if (recv.ContainsKey("lastname"))
            {
                query += "lastname=" + recv["lastname"] + ",";
            }

            if (recv.ContainsKey("icquin"))
            {
                query += "icquin=" + recv["icquin"] + ",";
            }

            if (recv.ContainsKey("homepage"))
            {
                query += "homepage=" + recv["homepage"] + ",";
            }

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
                        query += ", birthday="+d+",";

                        query += ", birthmonth="+ m+",";

                        query += ", birthyear="+y+",";
                    }
                }
                if (recv.ContainsKey("sex"))
                {
                    query += "sex="+ recv["sex"]+",";
                }
                if (recv.ContainsKey("zipcode"))
                {

                    query += ", zipcode=" + recv["zipcode"] + ",";
                }

                if (recv.ContainsKey("countrycode"))
                {
                    query += ", countrycode=" + recv["countrycode"] + ",";
                }
                UpdateProQuery.UpdateProfile(query);
            }
        }

        //if (recv.ContainsKey("aim"))
        //{
        //    if (recv["aim"] != session.PlayerInfo.PlayerAim)
        //    {
        //        query += ", aim=@P9";
        //        session.PlayerInfo.PlayerAim = recv["aim"];
        //        dataParams[9] = session.PlayerInfo.PlayerAim;
        //    }
        //}

        //if (recv.ContainsKey("pic"))
        //{
        //    int pic = 0;

        //    if (int.TryParse(recv["pic"], out pic))
        //    {
        //        if (pic != session.PlayerInfo.PlayerPicture)
        //        {
        //            query += ", picture=@P10";
        //            session.PlayerInfo.PlayerPicture = pic;
        //            dataParams[10] = pic;
        //        }
        //    }
        //}

        //if (recv.ContainsKey("occ"))
        //{
        //    int occ = 0;

        //    if (int.TryParse(recv["occ"], out occ))
        //    {
        //        if (occ != session.PlayerInfo.PlayerOccupation)
        //        {
        //            query += ", occupationid=@P11";
        //            session.PlayerInfo.PlayerOccupation = occ;
        //            dataParams[11] = occ;
        //        }
        //    }
        //}

        //if (recv.ContainsKey("ind"))
        //{
        //    int ind = 0;

        //    if (int.TryParse(recv["ind"], out ind))
        //    {
        //        if (ind != session.PlayerInfo.PlayerIndustryID)
        //        {
        //            query += ", industryid=@P12";
        //            session.PlayerInfo.PlayerIndustryID = ind;
        //            dataParams[12] = ind;
        //        }
        //    }
        //}

        //if (recv.ContainsKey("inc"))
        //{
        //    int inc = 0;

        //    if (int.TryParse(recv["inc"], out inc))
        //    {
        //        if (inc != session.PlayerInfo.PlayerIncomeID)
        //        {
        //            query += ", industryid=@P13";
        //            session.PlayerInfo.PlayerIncomeID = inc;
        //            dataParams[13] = inc;
        //        }
        //    }
        //}

        //if (recv.ContainsKey("mar"))
        //{
        //    int mar = 0;

        //    if (int.TryParse(recv["mar"], out mar))
        //    {
        //        if (mar != session.PlayerInfo.PlayerMarried)
        //        {
        //            query += ", marriedid=@P14";
        //            session.PlayerInfo.PlayerMarried = mar;
        //            dataParams[14] = mar;
        //        }
        //    }
        //}

        //if (recv.ContainsKey("chc"))
        //{
        //    int chc = 0;

        //    if (int.TryParse(recv["chc"], out chc))
        //    {
        //        if (chc != session.PlayerInfo.PlayerChildCount)
        //        {
        //            query += ", childcount=@P15";
        //            session.PlayerInfo.PlayerChildCount = chc;
        //            dataParams[15] = chc;
        //        }
        //    }
        //}

        //if (recv.ContainsKey("i1"))
        //{
        //    int i1 = 0;

        //    if (int.TryParse(recv["i1"], out i1))
        //    {
        //        if (i1 != session.PlayerInfo.PlayerInterests)
        //        {
        //            query += ", interests1=@P16";
        //            session.PlayerInfo.PlayerInterests = i1;
        //            dataParams[16] = i1;
        //        }
        //    }
        //}

        //if (recv.ContainsKey("nick"))
        //{
        //    if (recv["nick"] != session.PlayerInfo.PlayerNick)
        //    {
        //        query += ", nick=@P17";
        //        session.PlayerInfo.PlayerNick = recv["nick"];
        //        dataParams[17] = session.PlayerInfo.PlayerNick;
        //    }
        //}

        //if (recv.ContainsKey("uniquenick"))
        //{
        //    if (recv["uniquenick"] != session.PlayerInfo.PlayerUniqueNick)
        //    {
        //        query += ", uniquenick=@P18";
        //        session.PlayerInfo.PlayerHomepage = recv["uniquenick"];
        //        dataParams[18] = session.PlayerInfo.PlayerUniqueNick;
        //    }
        //}

        //try
        //{
        //    UpdateProQuery.UpdateUserInfo(query, dataParams);
        //}
        //catch (Exception e)
        //{
        //    LogWriter.Log.WriteException(e);
        //}
    }
}

