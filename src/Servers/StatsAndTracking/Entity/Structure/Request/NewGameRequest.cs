using StatsAndTracking.Abstraction.BaseClass;
using StatsAndTracking.Entity.Enumerate;
using System.Collections.Generic;

namespace StatsAndTracking.Entity.Structure.Request
{
    public class NewGameRequest : GStatsRequestBase
    {
        public NewGameRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GStatsErrorCode Parse()
        {
            var flag = base.Parse();
            if (flag != GStatsErrorCode.NoError)
            {
                return flag;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
