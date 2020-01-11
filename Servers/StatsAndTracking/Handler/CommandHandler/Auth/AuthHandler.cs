using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.Auth
{
    public class AuthHandler
    {
        public static void SendSessionKey(GstatsSession session, Dictionary<string, string> dict)
        {
            //we have to verify the challenge response from the game, the response challenge is computed as
            //len = sprintf(resp, "%d%s",g_crc32(challenge,(int)strlen(challenge)), gcd_secret_key);
            //MD5Digest((unsigned char *)resp, (unsigned int)len, md5val);
            //DOXCODE(respformat, sizeof(respformat) - 1, enc3);
            //len = sprintf(resp, respformat, gcd_gamename, md5val, gameport);
            string sendingBuffer = string.Format(@"\sesskey\{0}", dict["response"]);

            session.SendAsync(sendingBuffer);
        }
    }
}
