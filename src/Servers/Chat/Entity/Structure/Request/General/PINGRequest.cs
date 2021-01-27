using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class PINGRequest : ChatRequestBase
    {
        public PINGRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
