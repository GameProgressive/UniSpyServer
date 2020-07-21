using System;
using System.Collections.Generic;
using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Entity.Structure.Model
{
    public class GStatsRequestModelBase
    {
        protected Dictionary<string, string> _recv;

        public GStatsRequestModelBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }

        public GStatsErrorCode Parse()
        {
            return GStatsErrorCode.NoError;
        }
    }
}
