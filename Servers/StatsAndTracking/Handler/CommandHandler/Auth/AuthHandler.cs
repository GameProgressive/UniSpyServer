using System.Collections.Generic;

namespace StatsAndTracking.Handler.CommandHandler.Auth
{
    public class AuthHandler : CommandHandlerBase
    {
        GameSpyLib.Encryption.Crc16 _crc16 = new GameSpyLib.Encryption.Crc16(GameSpyLib.Encryption.Crc16Mode.Standard);

        public AuthHandler() : base()
        {
        }

        protected override void DataOperation(GStatsSession session, Dictionary<string, string> recv)
        {
            //we have to verify the challenge response from the game, the response challenge is computed as
            //len = sprintf(resp, "%d%s",g_crc32(challenge,(int)strlen(challenge)), gcd_secret_key);
            //MD5Digest((unsigned char *)resp, (unsigned int)len, md5val);
            //DOXCODE(respformat, sizeof(respformat) - 1, enc3);
            //len = sprintf(resp, respformat, gcd_gamename, md5val, gameport);

            // for now we do not check this
            session.PlayerData.SessionKey = (uint)new System.Random().Next(0, 2147483647);
        }

        protected override void ConstructResponse(GStatsSession session, Dictionary<string, string> recv)
        {
            _sendingBuffer = string.Format(@"\sesskey\{0}", session.PlayerData.SessionKey);
        }
    }
}
