using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;

namespace UniSpyServer.Chat.Entity.Structure.Request.General
{
    [RequestContract("NAMES")]
    public sealed class NamesRequest : ChannelRequestBase
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
