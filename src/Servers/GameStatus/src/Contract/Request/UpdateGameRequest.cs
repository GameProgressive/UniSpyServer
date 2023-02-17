using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    
    public sealed class UpdateGameRequest : RequestBase
    {
        public int? ConnectionID { get; private set; }
        public bool? IsDone { get; private set; }
        public bool? IsClientLocalStorageAvailable { get; private set; }
        public string GameData { get; private set; }
        public int? SessionKey { get; private set; }
        public UpdateGameRequest(string rawRequest) : base(rawRequest)
        {
            IsClientLocalStorageAvailable = false;
            IsDone = false;
        }
        public override void Parse()
        {
            base.Parse();

            if (PlayerData.ContainsKey("dl"))
            {
                IsClientLocalStorageAvailable = true;
            }

            if (!PlayerData.ContainsKey("done"))
            {
                throw new GSException("done is missing.");
            }


            if (PlayerData["done"] == "1")
            {
                IsDone = true;
            }
            else if (PlayerData["done"] == "0")
            {
                IsDone = false;
            }
            else
            {
                throw new GSException("done format is incorrect.");
            }


            int sessKey;
            if (!int.TryParse(PlayerData["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid int.");
            }
            SessionKey = sessKey;

            if (!PlayerData.ContainsKey("gamedata"))
            {
                throw new GSException("gamedata is missing.");
            }
            GameData = PlayerData["gamedata"];
            if (PlayerData.ContainsKey("connid"))
            {
                int connID;
                if (!int.TryParse(PlayerData["connid"], out connID))
                {
                    throw new GSException("connid is not a valid int.");
                }
                ConnectionID = connID;
            }
        }
    }
}