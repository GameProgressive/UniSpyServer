using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System.Collections.Generic;

namespace PresenceSearchPlayer.Handler.Others
{
    /// <summary>
    /// Get player information
    /// </summary>
    public class OthersHandler
    {
        public static object OthersList { get; private set; }

        /// <summary>
        /// Get profiles that have you on their buddy(friend) list.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="recv"></param>
        public static void SearchOtherBuddy(GPSPSession session, Dictionary<string, string> recv)
        {
            // TODO: Please finis this function
            //others\sesskey\profileid\namespace\
            string sendingbuffer; //= @"\others\\o\<profileid>\nick\<>\uniquenick\<>\first\<>\last\<>\email\<>\odone\";
            var temp = OthersQuery.GetOtherBuddy(recv);
           //@"\others\\o\13\nick\MyCrysis\uniquenick\xiaojiuwo\first\xiao\last\jiuwo\email\koujiangheng@live.cn\odone\final\");

            if (temp == null)
            {
                GameSpyUtils.SendGPError(session, GPErrorCode.DatabaseError, "No Math Found");
                return;
            }
            if (temp.Count == 1)
            {
                sendingbuffer = string.Format(@"\others\\o\{0}\nick\{1}\uniquenick\{2}\first\{3}\last\{4}\email\{5}\odone\final\", recv["profileid"], temp[0]["nick"], temp[0]["uniquenick"], temp[0]["firstname"], temp[0]["lastname"], temp[0]["email"]);
                
                session.SendAsync(sendingbuffer);
                return;
            }
            if (temp.Count > 1)
            {
                sendingbuffer = @"\others\\";
                foreach (Dictionary<string, object> info in temp)
                {
                    sendingbuffer += string.Format(@"o\{0}\nick\{1}\uniquenick\{2}\first\{3}\last\{4}\email\{5}\", recv["profileid"],info["nick"], info["uniquenick"], info["firstname"], info["lastname"], info["email"]);
                }
                session.SendAsync(sendingbuffer);
                return;
            }
            GameSpyUtils.SendGPError(session, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
