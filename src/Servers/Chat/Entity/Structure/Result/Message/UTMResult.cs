using Chat.Abstraction.BaseClass.Message;

namespace Chat.Entity.Structure.Result.Message
{
    internal sealed class UTMResult : ChatMsgResultBase
    {
        public string Name { get; set; }
        public UTMResult()
        {
        }
    }
}