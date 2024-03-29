using System.Collections.Generic;
using UniSpy.Server.GameStatus.Abstraction.BaseClass;


namespace UniSpy.Server.GameStatus.Contract.Request
{

    public sealed class UpdateGameRequest : RequestBase
    {
        public int? ConnectionID { get; private set; }
        public bool IsDone { get; private set; }
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string GameData { get; private set; }
        public Dictionary<string, string> GameDataKeyValues { get; private set; }
        public int? SessionKey { get; private set; }
        public UpdateGameRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            if (!KeyValues.ContainsKey("gamedata"))
            {
                throw new GameStatus.Exception("request must contians gamedata");
            }
            GameData = KeyValues["gamedata"];
            GameDataKeyValues = ConvertGameDataToKeyValues(GameData);

            if (KeyValues.ContainsKey("dl"))
            {
                IsClientLocalStorageAvailable = true;
            }

            if (!KeyValues.ContainsKey("done"))
            {
                throw new GameStatus.Exception("done is missing.");
            }


            if (KeyValues["done"] == "1")
            {
                IsDone = true;
            }
            else if (KeyValues["done"] == "0")
            {
                IsDone = false;
            }
            else
            {
                throw new GameStatus.Exception("done format is incorrect.");
            }


            if (!int.TryParse(KeyValues["sesskey"], out var sessKey))
            {
                throw new GameStatus.Exception("sesskey is not a valid int.");
            }
            SessionKey = sessKey;


            if (KeyValues.ContainsKey("connid"))
            {
                if (!int.TryParse(KeyValues["connid"], out var connID))
                {
                    throw new GameStatus.Exception("connid is not a valid int.");
                }
                ConnectionID = connID;
            }
        }
    }
}