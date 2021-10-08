using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;

namespace Chat.Entity.Structure.Request.General
{
    [RequestContract("NAMES")]
    internal sealed class NamesRequest : ChannelRequestBase
    {
        public new string ChannelName
        {
            get => base.ChannelName;
            set => base.ChannelName = value;
        }
        public NamesRequest(string rawRequest) : base(rawRequest)
        {
        }
        public NamesRequest() { }
    }
}
