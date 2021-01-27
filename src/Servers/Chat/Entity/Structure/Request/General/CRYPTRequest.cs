using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class CRYPTRequest : ChatRequestBase
    {
        public CRYPTRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string VersionID { get; protected set; }
        public string GameName { get; protected set; }
        //CRYPT des %d %s

        public override void Parse()
        {
            base.Parse();
            if(ErrorCode != ChatErrorCode.NoError)
            {
                ErrorCode = ChatErrorCode.Parse;
                return;
            }

            VersionID = _cmdParams[1];
            GameName = _cmdParams[2];
        }
    }
}
