using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.General
{
    public sealed class LoginResult : ResultBase
    {
        public int ProfileId { get; set; }
        public int UserID { get; set; }
        public LoginResult(){ }
    }
}
