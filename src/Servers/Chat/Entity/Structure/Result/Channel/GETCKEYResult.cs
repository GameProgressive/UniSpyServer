using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result
{
    public class GETCKEYResult : ChatResultBase
    {
        public Dictionary<string, string> NickNamesAndBFlags { get; protected set; }
        public string ChannelName { get; protected set; }
        public string Cookie { get; protected set; }
        public GETCKEYResult(string channelName, string cookie)
        {
            NickNamesAndBFlags = new Dictionary<string, string>();
            ChannelName = channelName;
            Cookie = cookie;
        }
    }
}
