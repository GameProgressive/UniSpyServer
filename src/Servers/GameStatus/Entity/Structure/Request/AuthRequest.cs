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

            if (!_request.ContainsKey("gamename") && !_request.ContainsKey("response"))
            {
                return GSError.Parse;
            }

            if (_request.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_request["port"], out port))
                {
                    return GSError.Parse;
                }
                Port = port;
            }

            GameName = _request["gamename"];

            return GSError.NoError;
        }
    }
}
