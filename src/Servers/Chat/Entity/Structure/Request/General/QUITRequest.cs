using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand
{
    public class QUITRequest : ChatRequestBase
    {
        public QUITRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string Reason { get; protected set; }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }


            if (_longParam == null)
            {
                return false;
            }

            Reason = _longParam;

            return true;
        }
    }
}
