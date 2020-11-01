using StatsTracking.Abstraction.BaseClass;
using StatsTracking.Entity.Enumerate;
using System.Collections.Generic;

namespace StatsTracking.Entity.Structure.Request
{
    public class NewGameRequest : STRequestBase
    {
        public NewGameRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override STError Parse()
        {
            var flag = base.Parse();
            if (flag != STError.NoError)
            {
                return flag;
            }

            return STError.NoError;
        }
    }
}
