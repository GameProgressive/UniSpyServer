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

            if (!KeyValues.ContainsKey("nick") || !KeyValues.ContainsKey("keyhash"))
            {
                return GSError.Parse;
            }

            if (KeyValues.ContainsKey("nick"))
            {
                Nick = KeyValues["nick"];
            }
            else if (KeyValues.ContainsKey("keyhash"))
            {
                KeyHash = KeyValues["keyhash"];
            }
            else
            {
                return GSError.Parse;
            }

            return GSError.NoError;
        }
    }
}
