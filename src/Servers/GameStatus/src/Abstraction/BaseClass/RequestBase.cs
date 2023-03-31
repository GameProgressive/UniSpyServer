using UniSpy.Server.GameStatus.Exception;
using System.Collections.Generic;
using System.Linq;
using UniSpy.Server.Core.Misc;

namespace UniSpy.Server.GameStatus.Abstraction.BaseClass
{
    public abstract class RequestBase : UniSpy.Server.Core.Abstraction.BaseClass.RequestBase
    {
        public new string CommandName { get => (string)base.CommandName; protected set => base.CommandName = value; }
        public new string RawRequest => (string)base.RawRequest;
        /// <summary>
        /// LocalId is used to indicate the request sequence in gamespy clients
        /// </summary>
        public int? LocalId { get; protected set; }
        public Dictionary<string, string> KeyValues { get; protected set; }

        public RequestBase(string rawRequest) : base(rawRequest)
        {
        }
        public static Dictionary<string, string> ConvertGameDataToKeyValues(string gameData)
        {
            var parts = gameData.Split("\u0001", System.StringSplitOptions.RemoveEmptyEntries);
            return GameSpyUtils.ConvertToKeyValue(parts);
        }
        public override void Parse()
        {
            // the localid lid is used to indicate the request sequence on the client side
            // we just respond it with same lid to client
            KeyValues = GameSpyUtils.ConvertToKeyValue(RawRequest);
            CommandName = KeyValues.Keys.First();

            if (KeyValues.ContainsKey("lid"))
            {
                if (!int.TryParse(KeyValues["lid"], out var localId))
                {
                    throw new GSException("localid format is incorrect.");
                }
                LocalId = localId;
            }
            //worms 3d use id not lid so we added an condition here
            if (KeyValues.ContainsKey("id"))
            {
                if (!int.TryParse(KeyValues["id"], out var localId))
                {
                    throw new GSException("localid format is incorrect.");
                }
                LocalId = localId;
            }
        }
    }
}
