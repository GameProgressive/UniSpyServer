using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result
{
    public class GETKEYResult : ChatResultBase
    {
        public string Cookie;
        public List<string> Flags;
        public ChatUserInfo UserInfo { get; protected set; }
        public GETKEYResult(ChatUserInfo userInfo, string cookie)
        {
            Flags = new List<string>();
            UserInfo = userInfo;
            Cookie = cookie;
        }
    }
}
