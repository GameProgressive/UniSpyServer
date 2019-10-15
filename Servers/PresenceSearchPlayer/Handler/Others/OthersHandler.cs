using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Others
{
    public class OthersHandler
    {
        public static object OthersList { get; private set; }

        /// <summary>
        /// Get profiles that have you on their buddy(friend) list.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="dict"></param>
        public static void SearchOtherBuddy(GPSPSession session, Dictionary<string, string> dict)
        {
            // TODO: Please finis this function
            //others\sesskey\profileid\namespace\
            string sendingbuffer; //= @"\others\o\nick\<>\uniquenick\<>\first\<>\last\<>\email\<>\odone\";
            var temp = OthersQuery.GetOtherBuddy(dict);
            if (temp == null)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, "No Math Found");
                return;
            }
            if (temp.Count == 1)
            {
                sendingbuffer = string.Format(@"\others\o\nick\{0}\uniquenick\{1}\first\{2}\last\{3}\email\{4}\odone\final\", temp[0]["nick"], temp[0]["uniquenick"], temp[0]["firstname"], temp[0]["lastname"], temp[0]["email"]);
                session.SendAsync(sendingbuffer);
                return;
            }
            if (temp.Count > 1)
            {
                sendingbuffer = @"\others\";
                foreach (Dictionary<string, object> info in temp)
                {
                    sendingbuffer += string.Format(@"o\nick\{0}\uniquenick\{1}\first\{1}\last\{2}\email\{3}\", info["nick"], info["uniquenick"], info["firstname"], info["lastname"], info["email"]);
                }
                session.SendAsync(sendingbuffer);
                return;
            }
            GameSpyUtils.SendGPError(session, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
