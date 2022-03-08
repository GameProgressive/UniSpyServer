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
        public Dictionary<string, string> KeyValues { get; protected set; }

        public RequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
            KeyValues = GameSpyUtils.ConvertToKeyValue(rawRequest);
            CommandName = KeyValues.Keys.First();
        }

        public override void Parse()
        {
            if (!KeyValues.ContainsKey("lid") && !KeyValues.ContainsKey("id"))
            {
                throw new GSException("namespaceid is missing.");
            }

            if (KeyValues.ContainsKey("lid"))
            {
                int operationID;
                if (!int.TryParse(KeyValues["lid"], out operationID))
                {
                    throw new GSException("namespaceid format is incorrect.");
                }
                OperationID = operationID;
            }
            //worms 3d use id not lid so we added an condition here
            else if (KeyValues.ContainsKey("id"))
            {
                int operationID;
                if (!int.TryParse(KeyValues["id"], out operationID))
                {
                    throw new GSException("namespaceid format is incorrect.");
                }
                OperationID = operationID;
            }
        }
    }
}
