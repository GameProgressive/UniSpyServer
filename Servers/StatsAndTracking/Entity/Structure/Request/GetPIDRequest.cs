using System;
using System.Collections.Generic;
using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Entity.Structure.Request
{
    public class GetPIDRequest : GStatsRequestBase
    {
        public string Nick { get; protected set; }
        public string KeyHash { get; protected set; }

        public GetPIDRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }

            if (!_request.ContainsKey("nick") || !_request.ContainsKey("keyhash"))
            {
                return GStatsErrorCode.Parse;
            }

            if (_request.ContainsKey("nick"))
            {
                Nick = _request["nick"];
            }
            else if (_request.ContainsKey("keyhash"))
            {
                KeyHash = _request["keyhash"];
            }
            else
            {
                return GStatsErrorCode.Parse;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
