using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class UserIPResult : ResultBase
    {
        public string RemoteIPAddress { get; set; }
        public UserIPResult()
        {
        }
    }
}
