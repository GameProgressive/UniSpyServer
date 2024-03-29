using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{
    
    public sealed class QuitRequest : RequestBase
    {
        public QuitRequest() { }
        public QuitRequest(string rawRequest) : base(rawRequest){ }

        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();

            if (_longParam is null)
            {
                throw new Chat.Exception("Quit reason is missing.");
            }

            Reason = _longParam;
        }
    }
}
