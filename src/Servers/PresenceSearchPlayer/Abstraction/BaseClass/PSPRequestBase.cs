using UniSpyLib.Abstraction.Interface;
using PresenceSearchPlayer.Entity.Enumerate;
using System.Collections.Generic;
using System.Linq;
using UniSpyLib.Abstraction.BaseClass;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public class PSPRequestBase : RequestBase
    {
        protected Dictionary<string, string> _recv;
        public new string RawRequest { get; protected set; }
        public new string CommandName { get; protected set; }
        public ushort OperationID { get; protected set; }


        public PSPRequestBase(Dictionary<string, string> recv)
        {
            _recv = recv;
            CommandName = recv.Keys.First();
        }

        public override object Parse()
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

            return GPErrorCode.NoError;
        }
    }
}
