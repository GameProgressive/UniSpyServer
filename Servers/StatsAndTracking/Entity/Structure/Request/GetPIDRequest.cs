using System;
using System.Collections.Generic;
using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Entity.Structure.Request
{
    public class GetPIDRequest : GStatsRequestBase
    {
        public string Nick { get; protected set; }
        public string KeyHash { get; protected set; }
        public GetPIDRequest(Dictionary<string, string> recv) : base(recv)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }

            if (!_recv.ContainsKey("nick") || !_recv.ContainsKey("keyhash"))
            {
                return GStatsErrorCode.Parse;
            }

            if (_recv.ContainsKey("nick"))
            {
                Nick = _recv["nick"];
            }
            else if (_recv.ContainsKey("keyhash"))
            {
                KeyHash = _recv["keyhash"];
            }
            else
            {
                return GStatsErrorCode.Parse;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
