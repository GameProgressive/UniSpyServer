using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyLib.Abstraction.BaseClass.RequestBase
    {
        public Dictionary<string, string> RequestKeyValues { get; protected set; }
        public new string RawRequest{ get => (string)base.RawRequest;
        }
        public new string CommandName{ get => (string)base.CommandName;
            protected set => base.CommandName = value; }
        public ushort OperationID { get; protected set; }
        public RequestBase(string rawRequest) : base(rawRequest)
        {
            RequestKeyValues = GameSpyUtils.ConvertToKeyValue(rawRequest);
        }

        public override void Parse()
        {
            CommandName = RequestKeyValues.Keys.First();

            if (RequestKeyValues.ContainsKey("id"))
            {
                ushort operationID;
                if (!ushort.TryParse(RequestKeyValues["id"], out operationID))
                {
                    throw new GPParseException("operation id is invalid.");
                }
                OperationID = operationID;
            }
        }
    }
}
