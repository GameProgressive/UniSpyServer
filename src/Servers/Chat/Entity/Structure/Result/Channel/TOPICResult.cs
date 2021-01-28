using Chat.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Entity.Structure.Result.Channel
{
    internal sealed class TOPICResult : ChatResultBase
    {
        public string ChannelName { get; set; }
        public string ChannelTopic { get; set; }

        public TOPICResult()
        {
        }
    }
}
