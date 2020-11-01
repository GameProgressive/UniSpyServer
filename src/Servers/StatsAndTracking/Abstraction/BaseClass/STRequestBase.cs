using StatsTracking.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace StatsTracking.Abstraction.BaseClass
{
    public class STRequestBase
    {
        protected Dictionary<string, string> _request;
        public uint OperationID { get; protected set; }
        public string CommandName { get; protected set; }
        public STRequestBase(Dictionary<string, string> request)
        {
            _request = request;
            CommandName = request.Keys.First();
        }

        public virtual STError Parse()
        {

            if (!_request.ContainsKey("lid") && !_request.ContainsKey("id"))
            {
                return STError.Parse;
            }

            if (_request.ContainsKey("lid"))
            {
                uint operationID;
                if (!uint.TryParse(_request["lid"], out operationID))
                {
                    return STError.Parse;
                }
                OperationID = operationID;
            }

            //worms 3d use id not lid so we added an condition here
            if (_request.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_request["id"], out operationID))
                {
                    return STError.Parse;
                }
                OperationID = operationID;
            }

            return STError.NoError;
        }
    }
}
