using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    /// <summary>
    /// Request: //auth\\gamename\%s\response\%s\port\%d\id\1 */
    /// </summary>
    public class AuthRequest : GSRequestBase
    {
        public string GameName { get; protected set; }
        public uint Port { get; protected set; }
        public AuthRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GSError Parse()
        {
            var flag = base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("gamename") && !_rawRequest.ContainsKey("response"))
            {
                return GSError.Parse;
            }

            if (_rawRequest.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_rawRequest["port"], out port))
                {
                    return GSError.Parse;
                }
                Port = port;
            }

            GameName = _rawRequest["gamename"];

            return GSError.NoError;
        }
    }
}
