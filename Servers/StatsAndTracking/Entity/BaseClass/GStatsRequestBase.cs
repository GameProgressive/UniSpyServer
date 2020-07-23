using System;
using System.Collections.Generic;
using StatsAndTracking.Entity.Enumerator;

namespace StatsAndTracking.Entity.Structure.Request
{
    public class GStatsRequestBase
    {
        protected Dictionary<string, string> _recv;
        public uint OperationID { get; protected set; }
        public GStatsRequestBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }

        public virtual GStatsErrorCode Parse()
        {

            if (!_recv.ContainsKey("lid") && !_recv.ContainsKey("id"))
            {
                return GStatsErrorCode.Parse;
            }

            if (_recv.ContainsKey("lid"))
            {
                uint operationID;
                if (!uint.TryParse(_recv["lid"], out operationID))
                {
                    return GStatsErrorCode.Parse;
                }
                OperationID = operationID;
            }

            //worms 3d use id not lid so we added an condition here
            if (_recv.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_recv["id"], out operationID))
                {
                    return GStatsErrorCode.Parse;
                }
                OperationID = operationID;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
