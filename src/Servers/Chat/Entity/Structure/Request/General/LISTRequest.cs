using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class LISTRequest : ChatRequestBase
    {
        public LISTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public bool IsSearchingChannel { get; protected set; }
        public bool IsSearchingUser { get; protected set; }
        public string Filter { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (!ErrorCode)
            {
                ErrorCode = false;
                return;
            }

            if (_cmdParams.Count == 0)
            {
                IsSearchingChannel = true;
                ErrorCode = true;
            }

            Filter = _cmdParams[0];
            ErrorCode = true;
        }

    }
}
