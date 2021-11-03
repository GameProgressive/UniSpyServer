using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.General
{
    public sealed class WhoIsResult : ResultBase
    {
        public List<string> JoinedChannelName { get; }
        public string NickName { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string PublicIPAddress { get; set; }
        public WhoIsResult()
        {
            JoinedChannelName = new List<string>();
        }
    }
}
