using GameSpyLib.Common;
using PresenceSearchPlayer.DatabaseQuery;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler
{
    public class CheckHandler
    {

        /// <summary>
        /// Validates a user's info, without logging into the account.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void CheckProfileid(GPSPClient client, Dictionary<string, string> dict)
        {
            // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
            //\cur\pid\<pid>\final
            //check is request recieved correct and convert password into our MD5 type
            bool isContiansAllKey = dict.ContainsKey("nick") && dict.ContainsKey("email") && (dict.ContainsKey("passenc") || dict.ContainsKey("pass"));
            bool isEmailCorrect = GameSpyUtils.IsEmailFormatCorrect(dict["email"]);

            if (!isContiansAllKey&&! isEmailCorrect)
            {
                GameSpyUtils.SendGPError(client, GPErrorCode.Parse, "Parsing error, please check input");
                return;
            }

            //Search pid in our database and return whether exist
            string sendingBuffer;
            int profileid = CheckQuery.GetProfileidFromNickEmailPassword(dict);
            if (profileid != 1)
            {                
                sendingBuffer = string.Format(@"\cur\0\pid\{0}\final\", profileid);
                client.Stream.SendAsync(sendingBuffer);
            }
            else
            {
                sendingBuffer = "No math found";
                GameSpyUtils.SendGPError(client, GPErrorCode.DatabaseError, sendingBuffer);
            }
        }
    }
}
