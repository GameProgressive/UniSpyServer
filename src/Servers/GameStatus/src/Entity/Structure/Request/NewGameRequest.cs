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
        public uint? ConnectionID { get; private set; }
        public uint? SessionKey { get; private set; }
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
            uint sessKey;
            if (!uint.TryParse(RequestKeyValues["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid uint.");
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
                uint connectionID;
                if (!uint.TryParse(RequestKeyValues["connid"], out connectionID))
                {
                    throw new GSException("connid format is incorrect.");
                }
                ConnectionID = connectionID;
            }
        }
    }
}
