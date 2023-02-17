using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.General
{
    public sealed class LoginResult : ResultBase
    {
        public int ProfileId { get; set; }
        public int UserID { get; set; }
        public LoginResult() { }
    }
}
