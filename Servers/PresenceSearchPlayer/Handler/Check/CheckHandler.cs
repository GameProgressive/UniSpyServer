using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Check
{
    public class CheckHandler
    {
        static Dictionary<string, object> _queryResult;
        static Dictionary<string, string> _recv;
        static GPErrorCode _errorCode;
        static string _sendingBuffer;
        static string _errorMsg;
        /// <summary>
        /// Validates a user's info, without logging into the account.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="recv"></param>
        public static void CheckProfileid(GPSPSession session, Dictionary<string, string> recv)
        {
            // \check\\nick\<nick>\email\<email>\partnerid\0\passenc\<passenc>\gamename\gmtest\final\
            //\cur\pid\<pid>\final
            //check is request recieved correct and convert password into our MD5 type
            bool isContiansAllKey = recv.ContainsKey("nick") && recv.ContainsKey("email") && (recv.ContainsKey("passenc") || recv.ContainsKey("pass"));
            if (!isContiansAllKey)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "Parsing error, please check input.");
                return;
            }
            bool isEmailCorrect = GameSpyUtils.IsEmailFormatCorrect(recv["email"]);
            if (!isEmailCorrect)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.Parse, "Email format not correct.");
                return;
            }


            //Search pid in our database and return whether exist
            string sendingBuffer;
            int profileid = CheckQuery.GetProfileidFromNickEmailPassword(recv);
            if (profileid != -1)
            {
                sendingBuffer = string.Format(@"\cur\0\pid\{0}\final\", profileid);
                session.SendAsync(sendingBuffer);
            }
            else
            {
                sendingBuffer = "No math found";
                GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, sendingBuffer);
            }
        }
        private static void IsContainAllKey()
        {
            if (dict.ContainsKey("nick") || dict.ContainsKey("email") || dict.ContainsKey("passenc"))
        }
    }
}
