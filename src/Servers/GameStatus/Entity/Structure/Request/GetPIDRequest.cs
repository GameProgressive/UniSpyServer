using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    public class GetPIDRequest : GSRequestBase
    {
        public string Nick { get; protected set; }
        public string KeyHash { get; protected set; }

        public GetPIDRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override object Parse()
        {
           var flag = (GSError)base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (!RequestKeyValues.ContainsKey("nick") || !RequestKeyValues.ContainsKey("keyhash"))
            {
                return GSError.Parse;
            }

            if (RequestKeyValues.ContainsKey("nick"))
            {
                Nick = RequestKeyValues["nick"];
            }
            else if (RequestKeyValues.ContainsKey("keyhash"))
            {
                KeyHash = RequestKeyValues["keyhash"];
            }
            else
            {
                return GSError.Parse;
            }

            return GSError.NoError;
        }
    }
}
