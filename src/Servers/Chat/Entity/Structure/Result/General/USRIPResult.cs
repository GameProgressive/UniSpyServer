using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class USRIPResult : ChatResultBase
    {
        public string RemoteIPAddress { get; set; }
        public USRIPResult()
        {
        }
    }
}
