using System;
using System.Collections.Generic;
using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Entity.Structure.Request
{
    public class AuthRequest : GStatsRequestBase

    {
        //auth\\gamename\%s\response\%s\port\%d\id\1 */
        public string GameName { get; protected set; }
        public uint Port { get; protected set; }
        public AuthRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }
            
            if (!_request.ContainsKey("gamename") && !_request.ContainsKey("response"))
            {
                return GStatsErrorCode.Parse;
            }
            
            if (_request.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_request["port"], out port))
                {
                    return GStatsErrorCode.Parse;
                }
                Port = port;
            }

            GameName = _request["gamename"];

            return GStatsErrorCode.NoError;
        }
    }
}
