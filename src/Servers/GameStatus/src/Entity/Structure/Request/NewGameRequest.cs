using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Contract;
using GameStatus.Entity.Exception;

namespace GameStatus.Entity.Structure.Request
{
    [RequestContract("newgame")]

    public sealed class NewGameRequest : RequestBase
    {
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string Challenge { get; private set; }
        public uint ConnectionID { get; private set; }
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();

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
