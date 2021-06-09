using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class WHOISResult : ChatResultBase
    {
        public List<string> JoinedChannelName { get; set; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PublicIPAddress { get; set; }
        public WHOISResult()
        {
            JoinedChannelName = new List<string>();
        }
    }
}
