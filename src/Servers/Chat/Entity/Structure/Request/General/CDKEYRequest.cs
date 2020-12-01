using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.ChatCommand.General
{
    public class CDKEYRequest : ChatRequestBase
    {
        public CDKEYRequest(string rawRequest) : base(rawRequest)
        {
        }

        public string CDKey { get; protected set; }

        public override object Parse()
        {
            if(!(bool)base.Parse())
            {
                return false;
            }

            CDKey = _cmdParams[0];
            return true;
        }
    }
}
