using UniSpyLib.Abstraction.Interface;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public class PSPRequestBase : IRequest
    {
        protected Dictionary<string, string> _rawRequest;
        public string CommandName;
        public ushort OperationID { get; protected set; }

        object IRequest.CommandName => CommandName;

        public PSPRequestBase(Dictionary<string, string> recv)
        {
            _rawRequest = recv;
            CommandName = recv.Keys.First();
        }

        public virtual GPErrorCode Parse()
        {
            if (_rawRequest.ContainsKey("id"))
            {
                ushort operationID;
                if (!ushort.TryParse(_rawRequest["id"], out operationID))
                {
                    return GPErrorCode.Parse;
                }
                OperationID = operationID;
            }

            return GPErrorCode.NoError;
        }

        object IRequest.Parse()
        {
            return Parse();
        }
    }
}
