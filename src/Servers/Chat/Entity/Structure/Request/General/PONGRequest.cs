using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class PONGRequest : ChatRequestBase
    {
        public PONGRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string EchoMessage { get; protected set; }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            EchoMessage = _longParam;
            return true;
        }
    }
}
