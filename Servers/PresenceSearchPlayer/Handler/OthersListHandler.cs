using GameSpyLib.Common;
using PresenceSearchPlayer.DatabaseQuery;
using PresenceSearchPlayer.Enumerator;
using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    class OthersListHandler
    {
        public static void SearchOtherBuddyList(GPSPClient client, Dictionary<string, string> dict)
        {
            //request: \otherslist\sesskey\<searcher's sesskey>\profileid\<searcher's pid>\numopids\<how many pid in his list>
            //\opids\|<opid1>|<opid2>|******\namespaceid\<>\gamename\<>\final\

            string[] opids = dict["opids"].TrimStart('|').Split('|');
            // response: @"\otherslist\o\<o>\uniquenick\<uniquenick>\oldone\final\";
            string sendingBuffer = @"\otherslist\";
            foreach (string pid in opids)
            {
                List<Dictionary<string, object>> temp = OthersListQuery.GetOtherBuddyList(dict, pid);
                if (temp==null)
                {
                    continue;
                }
                else
                {
                    sendingBuffer += string.Format(@"o\{0}\uniquenick\{1}\", temp[0]["profileid"], temp[0]["uniquenick"]);
                }
            }

            sendingBuffer += @"oldone\final\";

            client.Stream.SendAsync(sendingBuffer);




            //GameSpyUtils.PrintReceivedGPDictToLogger("otherslist", dict);
            //GameSpyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }



    }
}
