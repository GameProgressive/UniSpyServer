using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Model
{
    public class RequestModelBase
    {
        protected Dictionary<string, string> _recv;
        public string Nick { get; protected set; }
        public string Uniquenick { get; protected set; }
        public string Email { get; protected set; }
        public uint PartnerID { get; protected set; }
        public string PassEnc { get; protected set; }
        public string Password { get; protected set; }
        public string GameName { get; protected set; }
        public uint NamespaceID { get; protected set; }
        public uint ProfileID { get; protected set; }
        public ushort OperationID { get; protected set; }

        public RequestModelBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }

        public virtual GPErrorCode Parse()
        {
            if (_recv.ContainsKey("id"))
            {
                ushort operationID;
                if (!ushort.TryParse(_recv["id"], out operationID))
                {
                    return GPErrorCode.Parse;
                }
                OperationID = operationID;
            }

            if (_recv.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                {
                    return GPErrorCode.Parse;
                }

                NamespaceID = namespaceID;
            }

            return GPErrorCode.NoError;
        }
    }
}
