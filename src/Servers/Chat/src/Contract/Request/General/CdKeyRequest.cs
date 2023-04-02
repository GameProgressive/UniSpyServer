using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class CdKeyRequest : RequestBase
    {

        public string CdKey { get; private set; }
        public CdKeyRequest(string rawRequest) : base(rawRequest) { }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count < 1)
            {
                throw new Chat.Exception("The number of IRC cmdParams are incorrect.");
            }

            CdKey = _cmdParams[0];
        }
    }
}
