using UniSpy.Server.GameStatus.Abstraction.BaseClass;


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
            if (!KeyValues.ContainsKey("lid") && !KeyValues.ContainsKey("id"))
            {
                throw new GameStatus.Exception("localid is missing.");
            }

            if (!KeyValues.ContainsKey("nick") || !KeyValues.ContainsKey("keyhash"))
            {
                throw new GameStatus.Exception("nick or keyhash is missing.");
            }

            if (KeyValues.ContainsKey("nick"))
            {
                Nick = KeyValues["nick"];
            }
            
            if (KeyValues.ContainsKey("keyhash"))
            {
                KeyHash = KeyValues["keyhash"];
            }
        }
    }
}
