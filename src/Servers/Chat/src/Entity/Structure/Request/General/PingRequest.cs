using UniSpyServer.Servers.Chat.Abstraction.BaseClass;


namespace UniSpyServer.Servers.Chat.Entity.Structure.Request.General
{
    
    public sealed class PingRequest : RequestBase
    {
        public PingRequest(string rawRequest) : base(rawRequest){ }
    }
}
