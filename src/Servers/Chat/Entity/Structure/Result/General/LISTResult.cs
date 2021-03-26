using Chat.Abstraction.BaseClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.Entity.Structure.Result.General
{
    internal class LISTDataModel
    {
        public string ChannelName { get; set; }
        public int TotalChannelUsers { get; set; }
        public string ChannelTopic { get; set; }
    }

    internal sealed class LISTResult : ChatResultBase
    {
        public string UserIRCPrefix { get; set; }
        public List<LISTDataModel> ChannelInfos { get; set; }
        public LISTResult()
        {
            ChannelInfos = new List<LISTDataModel>();
        }
    }
}
