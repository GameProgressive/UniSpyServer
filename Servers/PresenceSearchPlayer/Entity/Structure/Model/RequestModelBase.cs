using System;
using System.Collections.Generic;
using PresenceSearchPlayer.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Model
{
    public class RequestModelBase
    {
        protected Dictionary<string, string> _recv;
        public string Nick { get; protected set; }
        public string Email { get; protected set; }
        public uint PartnetID { get; protected set; }
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

        public virtual bool Parse(out GPErrorCode errorCode)
        {
            errorCode = GPErrorCode.NoError;

            if (_recv.ContainsKey("id"))
            {
                ushort operationID;
                if (!ushort.TryParse(_recv["id"], out operationID))
                {
                    errorCode = GPErrorCode.Parse;
                    return false;
                }

                OperationID = operationID;
            }

            if (_recv.ContainsKey("namespaceid"))
            {
                uint namespaceID;
                if (!uint.TryParse(_recv["namespaceid"], out namespaceID))
                {
                    errorCode = GPErrorCode.Parse;
                    return false;
                }

                NamespaceID = namespaceID;
            }

            return true;
        }
    }
}
