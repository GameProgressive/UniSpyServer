using System.Collections.Generic;
using GameSpyLib.Common.Entity.Interface;
using StatsAndTracking.Entity.Structure.Request;

namespace StatsAndTracking.Handler.CommandHandler.Auth
{
    public class AuthHandler : GStatsCommandHandlerBase
    {
        //GameSpyLib.Encryption.Crc16 _crc16 = new GameSpyLib.Encryption.Crc16(GameSpyLib.Encryption.Crc16Mode.Standard);
        protected new AuthRequest _request;
        public AuthHandler(ISession session, Dictionary<string,string> recv) : base(session,recv)
        {
        }

        protected override void CheckRequest()
        {
            _errorCode = _request.Parse();
        }


        protected override void DataOperation()
        {
            //we have to verify the challenge response from the game, the response challenge is computed as
            //len = sprintf(resp, "%d%s",g_crc32(challenge,(int)strlen(challenge)), gcd_secret_key);
            //MD5Digest((unsigned char *)resp, (unsigned int)len, md5val);
            //DOXCODE(respformat, sizeof(respformat) - 1, enc3);
            //len = sprintf(resp, respformat, gcd_gamename, md5val, gameport);

            // for now we do not check this
            //session.PlayerData.SessionKey = (uint)new System.Random().Next(0, 2147483647);
            _session.PlayerData.SessionKey = 2020;
            _session.PlayerData.GameName = _request.GameName;
        }

        protected override void ConstructResponse()
        {
            _sendingBuffer = @$"\sesskey\{_session.PlayerData.SessionKey}";
            base.ConstructResponse();
        }

 
    }
}
