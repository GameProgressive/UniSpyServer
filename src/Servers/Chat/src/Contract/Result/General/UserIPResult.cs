using UniSpy.Server.Chat.Abstraction.BaseClass;

namespace UniSpy.Server.Chat.Contract.Result.General
{
    public sealed class UserIPResult : ResultBase
    {
        public string RemoteIPAddress { get; set; }
        public UserIPResult(){ }
    }
}
