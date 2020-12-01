using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using System.Collections.Generic;

namespace GameStatus.Entity.Structure.Request
{
    public class GetPIDRequest : GSRequestBase
    {
        public string Nick { get; protected set; }
        public string KeyHash { get; protected set; }

        public GetPIDRequest(Dictionary<string, string> request) : base(request)
        {
        }

        public override GSError Parse()
        {
            var flag = base.Parse();
            if (flag != GSError.NoError)
            {
                return flag;
            }

            if (!_rawRequest.ContainsKey("nick") || !_rawRequest.ContainsKey("keyhash"))
            {
                return GSError.Parse;
            }

            if (_rawRequest.ContainsKey("nick"))
            {
                Nick = _rawRequest["nick"];
            }
            else if (_rawRequest.ContainsKey("keyhash"))
            {
                KeyHash = _rawRequest["keyhash"];
            }
            else
            {
                return GSError.Parse;
            }

            return GSError.NoError;
        }
    }
}
