using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Result.Message
{
    internal sealed class ATMResult : ChatResultBase
    {
        public ChatUserInfo UserInfo { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }

        public ATMResult()
        {
        }
    }
}
