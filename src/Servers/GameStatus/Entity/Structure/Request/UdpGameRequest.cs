using GameStatus.Abstraction.BaseClass;
using GameStatus.Entity.Enumerate;

namespace GameStatus.Entity.Structure.Request
{
    internal sealed class UdpGameRequest : GSRequestBase
    {
        public uint ConnectionID { get; private set; }
        public bool IsDone { get; private set; }
        public bool IsClientLocalStorageAvailable { get; private set; }
        public string GameData { get; private set; }
        public UdpGameRequest(string rawRequest) : base(rawRequest)
        {
            IsClientLocalStorageAvailable = false;
            IsDone = false;
        }
        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != GSErrorCode.NoError)
            {
                return;
            }
            if (RequestKeyValues.ContainsKey("dl"))
            {
                IsClientLocalStorageAvailable = true;
            }

            if (!RequestKeyValues.ContainsKey("done"))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }
            bool isDone;
            if (!bool.TryParse(RequestKeyValues["done"], out isDone))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }
            IsDone = isDone;

            if (!RequestKeyValues.ContainsKey("gamedata"))
            {
                ErrorCode = GSErrorCode.Parse;
                return;
            }
            GameData = RequestKeyValues["gamedata"];

        }
    }
}