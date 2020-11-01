using StatsTracking.Abstraction.BaseClass;
using StatsTracking.Entity.Enumerate;
using System.Collections.Generic;

namespace StatsTracking.Entity.Structure.Request
{
    /// <summary>
    /// Request: //auth\\gamename\%s\response\%s\port\%d\id\1 */
    /// </summary>
    public class AuthRequest : STRequestBase
    {
        public string GameName { get; protected set; }
        public uint Port { get; protected set; }
        public AuthRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override STError Parse()
        {
            var flag = base.Parse();
            if (flag != STError.NoError)
            {
                return flag;
            }

            if (!_request.ContainsKey("gamename") && !_request.ContainsKey("response"))
            {
                return STError.Parse;
            }

            if (_request.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_request["port"], out port))
                {
                    return STError.Parse;
                }
                Port = port;
            }

            GameName = _request["gamename"];

            return STError.NoError;
        }
    }
}
