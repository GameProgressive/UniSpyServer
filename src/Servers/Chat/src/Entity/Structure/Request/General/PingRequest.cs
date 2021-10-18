using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("PING")]
    public sealed class PingRequest : RequestBase
    {
        public PingRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}
