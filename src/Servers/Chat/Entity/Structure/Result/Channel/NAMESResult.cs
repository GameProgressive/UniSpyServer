using Chat.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class NAMESResult : ChatResultBase
    {
        public string ChannelUserNickString { get; set; }
        public string ChannelName { get; set; }
        public string RequesterNickName { get; set; }
        public NAMESResult()
        {
        }
    }
}
