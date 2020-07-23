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
        public AuthRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.Parse)
            {
                return flag;
            }
            if (!_recv.ContainsKey("gamename") && !_recv.ContainsKey("response"))
            {
                return GStatsErrorCode.Parse;
            }
            
            if (_recv.ContainsKey("port"))
            {
                uint port;
                if (!uint.TryParse(_recv["port"], out port))
                {
                    return GStatsErrorCode.Parse;
                }
                Port = port;
            }

            GameName = _recv["gamename"];

            return GStatsErrorCode.NoError;
        }
    }
}
