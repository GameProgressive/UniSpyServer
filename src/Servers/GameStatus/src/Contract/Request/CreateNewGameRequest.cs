using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    

    public sealed class CreateNewGameRequest : RequestBase
    {
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string Challenge { get; private set; }
        public int? ConnectionID { get; private set; }
        public int? SessionKey { get; private set; }
        public CreateNewGameRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            if (!PlayerData.ContainsKey("sesskey"))
            {
                throw new GSException("sesskey is missing.");
            }
            int sessKey;
            if (!int.TryParse(PlayerData["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid int.");
            }
            SessionKey = sessKey;
            if (!PlayerData.ContainsKey("connid"))
            {
                throw new GSException("connid is missing.");
            }
            int connectionID;
            if (!int.TryParse(PlayerData["connid"], out connectionID))
            {
                throw new GSException("connid format is incorrect.");
            }
            ConnectionID = connectionID;

            if (PlayerData.ContainsKey("challenge"))
            {
                IsClientLocalStorageAvailable = true;
                Challenge = PlayerData["challenge"];
            }
        }
    }
}
