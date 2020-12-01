using GameStatus.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace GameStatus.Abstraction.BaseClass
{
    public class GSRequestBase : RequestBase
    {
        protected Dictionary<string, string> _rawRequest;
        public uint OperationID { get; protected set; }
        public new string CommandName { get; protected set; }
        public new Dictionary<string,string> RawRequest { get; protected set; }
        public GSRequestBase(Dictionary<string, string> request)
        {
            _rawRequest = request;
            CommandName = request.Keys.First();
        }

        public virtual GSError Parse()
        {

            if (!_rawRequest.ContainsKey("lid") && !_rawRequest.ContainsKey("id"))
            {
                return GSError.Parse;
            }

            if (_rawRequest.ContainsKey("lid"))
            {
                uint operationID;
                if (!uint.TryParse(_rawRequest["lid"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            //worms 3d use id not lid so we added an condition here
            if (_rawRequest.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_rawRequest["id"], out operationID))
                {
                    return GSError.Parse;
                }
                OperationID = operationID;
            }

            return GSError.NoError;
        }
    }
}
