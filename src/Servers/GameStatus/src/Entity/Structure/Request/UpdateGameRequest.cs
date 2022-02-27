using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Exception;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
{
    [RequestContract("updgame")]
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

            if (KeyValues.ContainsKey("dl"))
            {
                IsClientLocalStorageAvailable = true;
            }

            if (!KeyValues.ContainsKey("done"))
            {
                throw new GSException("done is missing.");
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
                throw new GSException("done format is incorrect.");
            }


            int sessKey;
            if (!int.TryParse(KeyValues["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid int.");
            }
            SessionKey = sessKey;

            if (!KeyValues.ContainsKey("gamedata"))
            {
                throw new GSException("gamedata is missing.");
            }
            GameData = KeyValues["gamedata"];
            if (KeyValues.ContainsKey("connid"))
            {
                int connID;
                if (!int.TryParse(KeyValues["connid"], out connID))
                {
                    throw new GSException("connid is not a valid int.");
                }
                ConnectionID = connID;
            }
        }
    }
}