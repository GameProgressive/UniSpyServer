using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class QUITRequest : ChatRequestBase
    {
        public QUITRequest() { }
        public QUITRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; set; }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.NoError;
                return;
            }


            if (_longParam == null)
            {
                ErrorCode = ChatErrorCode.NoError;
                return;
            }

            Reason = _longParam;
        }
    }
}
