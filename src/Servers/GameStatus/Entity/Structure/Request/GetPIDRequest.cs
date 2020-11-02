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

            if (!_request.ContainsKey("nick") || !_request.ContainsKey("keyhash"))
            {
                return GSError.Parse;
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
                return GSError.Parse;
            }

            return GSError.NoError;
        }
    }
}
