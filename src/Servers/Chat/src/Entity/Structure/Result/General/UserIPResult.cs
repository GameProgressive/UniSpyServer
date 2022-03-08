using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.General
{
    public sealed class UserIPResult : ResultBase
    {
        public string RemoteIPAddress { get; set; }
        public UserIPResult(){ }
    }
}
