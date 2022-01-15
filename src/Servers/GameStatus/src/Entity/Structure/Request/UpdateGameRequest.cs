using System;
using UniSpyServer.Servers.GameStatus.Abstraction.BaseClass;
using UniSpyServer.Servers.GameStatus.Entity.Contract;
using UniSpyServer.Servers.GameStatus.Entity.Exception;

namespace UniSpyServer.Servers.GameStatus.Entity.Structure.Request
{
    [RequestContract("updgame")]
    public sealed class UpdateGameRequest : RequestBase
    {
        public uint? ConnectionID { get; private set; }
        public bool? IsDone { get; private set; }
        public bool? IsClientLocalStorageAvailable { get; private set; }
        public string GameData { get; private set; }
        public uint? SessionKey { get; private set; }
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


            if (RequestKeyValues["done"] == "1")
            {
                IsDone = true;
            }
            else if (RequestKeyValues["done"] == "0")
            {
                IsDone = false;
            }
            else
            {
                throw new GSException("done format is incorrect.");
            }


            uint sessKey;
            if (!uint.TryParse(RequestKeyValues["sesskey"], out sessKey))
            {
                throw new GSException("sesskey is not a valid uint.");
            }
            SessionKey = sessKey;

            if (!RequestKeyValues.ContainsKey("gamedata"))
            {
                throw new GSException("gamedata is missing.");
            }
            GameData = RequestKeyValues["gamedata"];
            if (RequestKeyValues.ContainsKey("connid"))
            {
                uint connID;
                if (!uint.TryParse(RequestKeyValues["connid"], out connID))
                {
                    throw new GSException("connid is not a valid uint.");
                }
                ConnectionID = connID;
            }
        }
    }
}