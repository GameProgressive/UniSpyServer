using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
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
            if (ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_cmdParams.Count == 0)
            {
                IsSearchingChannel = true;
                ErrorCode = ChatErrorCode.Parse;
            }

            Filter = _cmdParams[0];
        }

    }
}
