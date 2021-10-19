using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Contract;
using GameStatus.Entity.Exception;

namespace GameStatus.Entity.Structure.Request
{
    [RequestContract("updgame")]
    public sealed class UpdateGameRequest : RequestBase
    {
        public uint ConnectionID { get; private set; }
        public bool IsDone { get; private set; }
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string GameData { get; private set; }
        public UpdateGameRequest(string rawRequest) : base(rawRequest)
        {
            IsClientLocalStorageAvailable = false;
            IsDone = false;
        }
        public override void Parse()
        {
            base.Parse();

            if (RequestKeyValues.ContainsKey("dl"))
            {
                IsClientLocalStorageAvailable = true;
            }

            if (!RequestKeyValues.ContainsKey("done"))
            {
                throw new GSException("done is missing.");
            }
            bool isDone;
            if (!bool.TryParse(RequestKeyValues["done"], out isDone))
            {
                throw new GSException("done format is incorrect.");
            }
            IsDone = isDone;

            if (!RequestKeyValues.ContainsKey("gamedata"))
            {
                throw new GSException("gamedata is missing.");
            }
            GameData = RequestKeyValues["gamedata"];

        }
    }
}