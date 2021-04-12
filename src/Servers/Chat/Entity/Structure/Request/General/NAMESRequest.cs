using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class NAMESRequest : ChatChannelRequestBase
    {
        public NAMESRequest(string rawRequest) : base(rawRequest)
        {
        }
        public NAMESRequest() { }
    }
}
