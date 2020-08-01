using System.Collections.Generic;
using PresenceSearchPlayer.Entity.Enumerator;

namespace PresenceSearchPlayer.Entity.Structure.Model
{
    public class PSPRequestBase
    {
        protected Dictionary<string, string> _recv;

        public ushort OperationID { get; protected set; }

        public PSPRequestBase(Dictionary<string, string> recv)
        {
            _recv = recv;
        }

        public virtual GPError Parse()
        {
            if (_recv.ContainsKey("id"))
            {
                ushort operationID;
                if (!ushort.TryParse(_recv["id"], out operationID))
                {
                    return GPError.Parse;
                }
                OperationID = operationID;
            }

            return GPError.NoError;
        }
    }
}
