using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;

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
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }
            if (RequestKeyValues.ContainsKey("connid") && RequestKeyValues.ContainsKey("challenge")
            || !RequestKeyValues.ContainsKey("connid") && !RequestKeyValues.ContainsKey("challenge"))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
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
                    ErrorCode = GSErrorCode.Parse;
                    return;
                }
                ConnectionID = connectionID;
            }
        }
    }
}
