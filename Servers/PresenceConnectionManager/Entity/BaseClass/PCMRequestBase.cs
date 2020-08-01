using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceConnectionManager.Entity.BaseClass
{
    public abstract class PCMRequestBase
    {
        protected Dictionary<string, string> _recv;
        //public uint NamespaceID { get; protected set; }
        public uint OperationID { get; protected set; }

        public PCMRequestBase(Dictionary<string,string> recv)
        {
            _recv = recv;
        }

        public virtual GPError  Parse()
        {
            if (_recv.ContainsKey("id"))
            {
                uint operationID;
                if (!uint.TryParse(_recv["id"], out operationID))
                {
                   return GPError.Parse;
                }
                OperationID = operationID;
            }

            //if (_recv.ContainsKey("namespaceid"))
            //{
            //    uint namespaceID;
            //    if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
            //    {
            //        return GPErrorCode.Parse;
            //    }
            //    NamespaceID = namespaceID;
            //}

            return GPError.NoError;
        }
    }
}
