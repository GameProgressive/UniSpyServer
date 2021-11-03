using UniSpyServer.Servers.Chat.Entity.Exception;

namespace UniSpyServer.Servers.Chat.Abstraction.BaseClass
{
    public class ChannelRequestBase : RequestBase
    {
        public string ChannelName { get; set; }
        public ChannelRequestBase() { }
        public ChannelRequestBase(string rawRequest) : base(rawRequest)
        {
        }
        public override void Parse()
        {
            base.Parse();

            if (_cmdParams.Count < 1)
            {
                throw new Exception("channel name is missing.");
            }
            ChannelName = _cmdParams[0];
        }
    }
}
