using UniSpyServer.Servers.GameStatus.Entity.Exception;
using System.Collections.Generic;
using System.Linq;
using UniSpyServer.UniSpyLib.MiscMethod;

namespace UniSpyServer.Servers.GameStatus.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpyLib.Abstraction.BaseClass.RequestBase
    {
        public int OperationID { get; protected set; }
        public new string CommandName { get => (string)base.CommandName; set => base.CommandName = value; }
        public new string RawRequest { get => (string)base.RawRequest; set => base.RawRequest = value; }
        public Dictionary<string, string> RequestKeyValues { get; protected set; }

        public RequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
            RequestKeyValues = GameSpyUtils.ConvertToKeyValue(rawRequest);
            CommandName = RequestKeyValues.Keys.First();
        }

        public override void Parse()
        {
            if (!RequestKeyValues.ContainsKey("lid") && !RequestKeyValues.ContainsKey("id"))
            {
                throw new GSException("namespaceid is missing.");
            }

            if (RequestKeyValues.ContainsKey("lid"))
            {
                int operationID;
                if (!int.TryParse(RequestKeyValues["lid"], out operationID))
                {
                    throw new GSException("namespaceid format is incorrect.");
                }
                OperationID = operationID;
            }
            //worms 3d use id not lid so we added an condition here
            else if (RequestKeyValues.ContainsKey("id"))
            {
                int operationID;
                if (!int.TryParse(RequestKeyValues["id"], out operationID))
                {
                    throw new GSException("namespaceid format is incorrect.");
                }
                OperationID = operationID;
            }
        }
    }
}
