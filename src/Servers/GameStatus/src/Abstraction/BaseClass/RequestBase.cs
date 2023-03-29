using UniSpy.Server.GameStatus.Exception;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.GameStatus.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        /// <summary>
        /// LocalId is used to indicate the request sequence in gamespy clients
        /// </summary>
        public int? LocalId { get; protected set; }
        public new string CommandName { get => (string)base.CommandName; set => base.CommandName = value; }
        public new string RawRequest { get => (string)base.RawRequest; set => base.RawRequest = value; }
        public Dictionary<string, string> KeyValues { get; protected set; }

        public RequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
        }

        public override void Parse()
        {
            CommandName = GameSpyUtils.GetRequestName(RawRequest);
            // the localid lid is used to indicate the request sequence on the client side
            // we just respond it with same lid to client
            KeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);
            if (!KeyValues.ContainsKey("lid") && !KeyValues.ContainsKey("id"))
            {
                throw new GSException("localid is missing.");
            }

            if (KeyValues.ContainsKey("lid"))
            {
                int operationID;
                if (!int.TryParse(KeyValues["lid"], out operationID))
                {
                    throw new GSException("localid format is incorrect.");
                }
                LocalId = operationID;
            }
            //worms 3d use id not lid so we added an condition here
            else
            {
                int operationID;
                if (!int.TryParse(KeyValues["id"], out operationID))
                {
                    throw new GSException("localid format is incorrect.");
                }
                LocalId = operationID;
            }
        }
    }
}
