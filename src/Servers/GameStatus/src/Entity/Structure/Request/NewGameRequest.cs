using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Exception;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
{
    [RequestContract("newgame")]

    public sealed class NewGameRequest : RequestBase
    {
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string Challenge { get; private set; }
        public int? ConnectionID { get; private set; }
        public int? SessionKey { get; private set; }
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();
            if (!RequestKeyValues.ContainsKey("sesskey"))
            {
                throw new GSException("sesskey is missing.");
            }
            int sessKey;
            if (!int.TryParse(RequestKeyValues["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid int.");
            }
            SessionKey = sessKey;

            if (RequestKeyValues.ContainsKey("connid") && RequestKeyValues.ContainsKey("challenge")
            || !RequestKeyValues.ContainsKey("connid") && !RequestKeyValues.ContainsKey("challenge"))
            {
                throw new GSException("newgame request is invalid.");
            }
            if (RequestKeyValues.ContainsKey("challenge"))
            {
                IsClientLocalStorageAvailable = true;
                Challenge = RequestKeyValues["challenge"];
            }
            else
            {
                int connectionID;
                if (!int.TryParse(RequestKeyValues["connid"], out connectionID))
                {
                    throw new GSException("connid format is incorrect.");
                }
                ConnectionID = connectionID;
            }
        }
    }
}
