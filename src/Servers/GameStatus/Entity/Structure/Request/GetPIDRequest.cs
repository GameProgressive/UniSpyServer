using StatsTracking.Abstraction.BaseClass;
using StatsTracking.Entity.Enumerate;
using System.Collections.Generic;

namespace StatsTracking.Entity.Structure.Request
{
    public class GetPIDRequest : STRequestBase
    {
        public string Nick { get; protected set; }
        public string KeyHash { get; protected set; }

        public GetPIDRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override STError Parse()
        {
            var flag = base.Parse();
            if (flag != STError.NoError)
            {
                return flag;
            }

            if (!_request.ContainsKey("nick") || !_request.ContainsKey("keyhash"))
            {
                return STError.Parse;
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
                return STError.Parse;
            }

            return STError.NoError;
        }
    }
}
