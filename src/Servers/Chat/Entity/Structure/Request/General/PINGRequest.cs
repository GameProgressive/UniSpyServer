using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    internal sealed class PINGRequest : ChatRequestBase
    {
        public PINGRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
