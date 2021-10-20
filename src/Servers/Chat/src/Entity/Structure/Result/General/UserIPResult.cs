using UniSpyServer.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Result.General
{
    public sealed class UserIPResult : ResultBase
    {
        public string RemoteIPAddress { get; set; }
        public UserIPResult()
        {
        }
    }
}
