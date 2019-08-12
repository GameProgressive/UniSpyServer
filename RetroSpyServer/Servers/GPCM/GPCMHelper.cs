using GameSpyLib.Extensions;
using RetroSpyServer.Servers.GPCM.Enumerator;
using RetroSpyServer.Servers.GPCM.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace RetroSpyServer.Servers.GPCM
{
    public class GPCMHelper
    {
        /// <summary>
        /// Generates an MD5 hash, which is used to verify the clients login information
        /// </summary>
        /// <param name="challenge1">First challenge key</param>
        /// <param name="challenge2">Second challenge key</param>
        /// <param name="userdata">The user data to append to the proof</param>
        /// <param name="partnerid">The partnerid to append</param>
        /// <returns>
        ///     The proof verification MD5 hash string that can be compared to what the client sends,
        ///     to verify that the users entered password matches the specific user data in the database.
        /// </returns>
        public static string GenerateProof(string challenge1, string challenge2, string userdata, uint partnerid, GPCMPlayerInfo playerinfo)
        {
            string realUserData = userdata;

            if (partnerid != (uint)PartnerID.Gamespy)
            {
                realUserData = string.Format("{0}@{1}", partnerid, userdata);
            }

            // Generate our string to be hashed
            StringBuilder HashString = new StringBuilder(playerinfo.PasswordHash);
            HashString.Append(' ', 48); // 48 spaces
            HashString.Append(realUserData);
            HashString.Append(challenge1);
            HashString.Append(challenge2);
            HashString.Append(playerinfo.PasswordHash);
            return HashString.ToString().GetMD5Hash();
        }

        /// <summary>
        /// This method is called when the server needs to send the buddies to the client
        /// </summary>
        public static void SendBuddies(GPCMClient client)
        {
            if (client.BuddiesSent)
                return;

            /*Stream.SendAsync(
                @"\bdy\1\list\2,\final\");

            Stream.SendAsync(
            //    @"\bm\100\f\2\msg\|s|0|ss|Offline\final\"
            @"\bm\100\f\2\msg\Messaggio di prova|s|2|ss|Home|ls|locstr://Reversing the world...|\final\"
            );*/

            client.Stream.SendAsync(@"\bdy\0\list\\final\");

            client.BuddiesSent = true;
        }

    }
}
