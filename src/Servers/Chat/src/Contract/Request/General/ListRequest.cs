using UniSpy.Server.Chat.Abstraction.BaseClass;


namespace UniSpy.Server.Chat.Contract.Request.General
{

    public sealed class ListRequest : RequestBase
    {
        public ListRequest(string rawRequest) : base(rawRequest) { }

        public bool IsSearchingChannel { get; private set; }
        public bool IsSearchingUser { get; private set; }
        public string Filter { get; private set; }

        public override void Parse()
        {
            base.Parse();

            if (_cmdParams is null || _cmdParams?.Count == 0)
            {
                throw new Chat.Exception("The Search filter is missing.");
            }

            IsSearchingChannel = true;
            Filter = _cmdParams[0];
        }
    }
}
