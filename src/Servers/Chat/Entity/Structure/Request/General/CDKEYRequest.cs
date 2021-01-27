using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class CDKEYRequest : ChatRequestBase
    {
        public CDKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string CDKey { get; protected set; }

        public override void Parse()
        {
            base.Parse();
            if(ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
            }

            CDKey = _cmdParams[0];
        }
    }
}
