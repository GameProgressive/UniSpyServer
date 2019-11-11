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
    }
}

