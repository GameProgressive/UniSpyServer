using UniSpy.Server.GameStatus.Abstraction.BaseClass;
using UniSpy.Server.GameStatus.Exception;

namespace UniSpy.Server.GameStatus.Contract.Request
{
    
    public sealed class GetProfileIDRequest : RequestBase
    {
        public string Nick { get; private set; }
        public string KeyHash { get; private set; }

        public GetProfileIDRequest(string rawRequest) : base(rawRequest)
        {
        }

        public override void Parse()
        {
            base.Parse();

            if (!PlayerData.ContainsKey("nick") || !PlayerData.ContainsKey("keyhash"))
            {
                throw new GSException("nick or keyhash is missing.");
            }

            if (PlayerData.ContainsKey("nick"))
            {
                Nick = PlayerData["nick"];
            }
            
            if (PlayerData.ContainsKey("keyhash"))
            {
                KeyHash = PlayerData["keyhash"];
            }
        }
    }
}
