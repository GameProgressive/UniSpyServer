using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Exception;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
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
            if (!KeyValues.ContainsKey("sesskey"))
            {
                throw new GSException("sesskey is missing.");
            }
            int sessKey;
            if (!int.TryParse(KeyValues["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid int.");
            }
            SessionKey = sessKey;
            if (!KeyValues.ContainsKey("connid"))
            {
                throw new GSException("connid is missing.");
            }
            int connectionID;
            if (!int.TryParse(KeyValues["connid"], out connectionID))
            {
                throw new GSException("connid format is incorrect.");
            }
            ConnectionID = connectionID;

            if (KeyValues.ContainsKey("challenge"))
            {
                IsClientLocalStorageAvailable = true;
                Challenge = KeyValues["challenge"];
            }
        }
    }
}
