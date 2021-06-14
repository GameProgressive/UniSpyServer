using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Request.General
{
    public class NAMESRequest : ChatChannelRequestBase
    {
        public new string ChannelName
        {
            get => base.ChannelName;
            set => base.ChannelName = value;
        }
        public NAMESRequest(string rawRequest) : base(rawRequest)
        {
        }
        public NAMESRequest() { }
    }
}
