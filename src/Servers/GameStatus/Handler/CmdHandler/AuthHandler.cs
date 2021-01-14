using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Structure.Request;
using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Handler.CmdHandler
{
    internal sealed class AuthHandler : GSCmdHandlerBase
    {
        //UniSpyLib.Encryption.Crc16 _crc16 = new UniSpyLib.Encryption.Crc16(UniSpyLib.Encryption.Crc16Mode.Standard);
        private new AuthRequest _request => (AuthRequest)base._request;
        private new AuthResult _result
        {
            get { return (AuthResult)base._result; }
            set { base._result = value; }
        }
        public AuthHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
        protected override void RequestCheck()
        {
            _result = new AuthResult();
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

        protected override void ResponseConstruct()
        {
            _response = new AuthResponse(_request, _result);
        }


    }
}
