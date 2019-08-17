using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceSearchPlayer.Handler
{
    public class OthersHandler
    {
        public static void SearchOtherBuddy(GPSPClient client, Dictionary<string, string> dict)
        {


            // TODO: Please finis this function
            string sendingbuffer = @"\otherslist\odone\";
            client.Stream.SendAsync(sendingbuffer);

            //GamespyUtils.PrintReceivedGPDictToLogger("others", dict);
            //GamespyUtils.SendGPError(client.Stream, GPErrorCode.General, "This request is not supported yet.");
        }

    }
}
