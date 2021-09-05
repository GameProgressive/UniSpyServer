using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class LOGINResult : ChatResultBase
    {
        public uint ProfileID { get; set; }
        public uint UserID { get; set; }
        public LOGINResult()
        {
        }
    }
}
