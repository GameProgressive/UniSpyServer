using UniSpy.Server.GameStatus.Entity.Exception;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.MiscMethod;

namespace UniSpy.Server.GameStatus.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        public int OperationID { get; protected set; }
        public new string CommandName { get => (string)base.CommandName; set => base.CommandName = value; }
        public new string RawRequest { get => (string)base.RawRequest; set => base.RawRequest = value; }
        public Dictionary<string, string> PlayerData { get; protected set; }

        public RequestBase(string rawRequest) : base(rawRequest)
        {
            RawRequest = rawRequest;
            PlayerData = GameSpyUtils.ConvertToKeyValue(rawRequest);
            CommandName = PlayerData.Keys.First();
        }

        public override void Parse()
        {
            if (!PlayerData.ContainsKey("lid") && !PlayerData.ContainsKey("id"))
            {
                throw new GSException("namespaceid is missing.");
            }

            if (PlayerData.ContainsKey("lid"))
            {
                int operationID;
                if (!int.TryParse(PlayerData["lid"], out operationID))
                {
                    throw new GSException("namespaceid format is incorrect.");
                }
                OperationID = operationID;
            }
            //worms 3d use id not lid so we added an condition here
            else if (PlayerData.ContainsKey("id"))
            {
                int operationID;
                if (!int.TryParse(PlayerData["id"], out operationID))
                {
                    throw new GSException("namespaceid format is incorrect.");
                }
                OperationID = operationID;
            }
        }
    }
}
