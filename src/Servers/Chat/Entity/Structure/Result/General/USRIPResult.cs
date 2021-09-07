using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class USRIPResult : ResultBase
    {
        public string RemoteIPAddress { get; set; }
        public USRIPResult()
        {
        }
    }
}
