using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request
{
    public class PINGRequest : ChatRequestBase
    {
        public PINGRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
