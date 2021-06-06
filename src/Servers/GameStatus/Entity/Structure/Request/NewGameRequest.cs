using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;
using GameStatus.Entity.Exception;

namespace GameStatus.Entity.Structure.Request
{
    internal sealed class NewGameRequest : GSRequestBase
    {
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string Challenge { get; private set; }
        public uint ConnectionID { get; private set; }
        public NewGameRequest(string rawRequest) : base(rawRequest)
        {
            IsClientLocalStorageAvailable = false;
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
