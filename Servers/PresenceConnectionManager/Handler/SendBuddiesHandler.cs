using System;
using System.Collections.Generic;
using System.Text;

namespace PresenceConnectionManager.Handler
{
    public class SendBuddiesHandler
    {
        public static void Handle(GPCMClient client)
        {
            //TODO
            if (client.BuddiesSent)
                return;

            /*Stream.SendAsync(
                @"\bdy\1\list\2,\final\");

            Stream.SendAsync(
            //    @"\bm\100\f\2\msg\|s|0|ss|Offline\final\"
            @"\bm\100\f\2\msg\Messaggio di prova|s|2|ss|Home|ls|locstr://Reversing the world...|\final\"
            );*/

            client.Stream.SendAsync(@"\bdy\1\list\1\final\");
            //client.Stream.SendAsync(@"\bm\100\f\13\msg\|s|0|ss|Offline\final\");
            client.BuddiesSent = true;
        }
    }
}
