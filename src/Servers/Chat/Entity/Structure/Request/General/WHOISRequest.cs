using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class WHOISRequest : ChatRequestBase
    {
        public WHOISRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string NickName { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if (ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            if (_cmdParams.Count != 1)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            NickName = _cmdParams[0];
        }
    }
}
