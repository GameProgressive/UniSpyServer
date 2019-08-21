using GameSpyLib.Common;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    public class OthersHandler
    {
        /// <summary>
        /// Get profiles that have you on their buddy(friend) list.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="dict"></param>
        public static void SearchOtherBuddy(GPSPClient client, Dictionary<string, string> dict)
        {
            // TODO: Please finis this function
            //others\sesskey\profileid\namespace\
            string sendingbuffer; //= @"\others\o\nick\<>\uniquenick\<>\first\<>\last\<>\email\<>\odone\";
            List<Dictionary<string,object>> temp = GPSPHandler.DBQuery.GetOtherBuddy(dict);
            if (temp.Count < 0)
            {
                GameSpyUtils.SendGPError(client.Stream, GPErrorCode.DatabaseError, "No Math Found");
                return;
            }
            if (temp.Count == 1)
            {
                sendingbuffer = string.Format(@"\others\o\nick\{0}\uniquenick\{1}\first\{1}\last\{2}\email\{3}\odone\final\",temp[0]["nick"],temp[0]["uniquenick"],temp[0]["firstname"],temp[0]["lastname"],temp[0]["email"]);
                client.Stream.SendAsync(sendingbuffer);
                return;
            }
            if(temp.Count>1)
            {
                sendingbuffer=@"\others\";
                foreach (Dictionary<string, object> info in temp)
                {
                    sendingbuffer += string.Format(@"o\nick\{0}\uniquenick\{1}\first\{1}\last\{2}\email\{3}\",info["nick"],info["uniquenick"],info["firstname"],info["lastname"],info["email"]);
                }
                client.Stream.SendAsync(sendingbuffer);
                return;
            }
           
            GameSpyUtils.PrintReceivedGPDictToLogger("others", dict);
            GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
