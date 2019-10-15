using System.Collections.Generic;

namespace StatsAndTracking.Handler.Auth
{
    public class AuthHandler
    {
        public static void SendSessionKey(GstatsSession session, Dictionary<string, string> dict)
        {
            string sendingBuffer = string.Format(@"\sesskey\{0}", dict["response"]);

            session.SendAsync(sendingBuffer);
        }
    }
}
