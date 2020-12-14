using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand.General
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
            if (!ErrorCode)
            {
               ErrorCode = false;
            }

            CDKey = _cmdParams[0];
            ErrorCode = true;
        }
    }
}
