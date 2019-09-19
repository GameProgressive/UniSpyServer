using System;
using System.Collections.Generic;
using PresenceSearchPlayer;
using GameSpyLib.Extensions;
using GameSpyLib.Common;
using GameSpyLib.Logging;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Handler
{
    /// <summary>
    /// This class contians common functions which helps server. 
    /// </summary>
    public class GPSPHandler
    {   
        /// <summary>
        ///  Format the password for our database storage
        /// </summary>
        /// <param name="dict"></param>
        public static void ProessPassword(Dictionary<string, string> dict)
        {
            string password;
            if (dict.ContainsKey("passenc"))
            {
                //we do nothing with encoded password                
                password = GameSpyUtils.DecodePassword(dict["passenc"]);
                dict["passenc"] = StringExtensions.GetMD5Hash(password);

            }
            else
            {
                password = dict["pass"];
                //password = GameSpyUtils.DecodePassword(dict["pass"]);
                dict["pass"] = StringExtensions.GetMD5Hash(password);
                dict.Add("passenc", dict["pass"]);
            }
        }
    }
}
