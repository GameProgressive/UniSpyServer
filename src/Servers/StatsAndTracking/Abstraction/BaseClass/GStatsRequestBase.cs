using StatsAndTracking.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace StatsAndTracking.Abstraction.BaseClass
{
    public class GStatsRequestBase
    {
        protected Dictionary<string, string> _request;
        public uint OperationID { get; protected set; }
        public string CommandName { get; protected set; }
        public GStatsRequestBase(Dictionary<string, string> request)
        {
            _request = request;
            CommandName = request.Keys.First();
        }

        public virtual GStatsErrorCode Parse()
        {

            if (!_request.ContainsKey("lid") && !_request.ContainsKey("id"))
            {
                return GStatsErrorCode.Parse;
            }

            if (_request.ContainsKey("lid"))
            {
                uint operationID;
                if (!uint.TryParse(_request["lid"], out operationID))
                {
                    return GStatsErrorCode.Parse;
                }
                OperationID = operationID;
            }

            //worms 3d use id not lid so we added an condition here
            if (_request.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_request["id"], out operationID))
                {
                    return GStatsErrorCode.Parse;
                }
                OperationID = operationID;
            }

            return GStatsErrorCode.NoError;
        }
    }
}
