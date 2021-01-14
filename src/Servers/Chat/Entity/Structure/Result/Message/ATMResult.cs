using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;

namespace Chat.Entity.Structure.Result
{
    public class ATMResult : ChatResultBase
    {
        public ChatUserInfo UserInfo { get; protected set; }
        public string Name { get; protected set; }
        public string Message { get; protected set; }

        public ATMResult(ChatUserInfo userInfo, string name, string message)
        {
            UserInfo = userInfo;
            Name = name;
            Message = message;
        }
    }
}
