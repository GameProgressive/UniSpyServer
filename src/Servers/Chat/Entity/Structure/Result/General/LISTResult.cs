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
        public string ChannelName;
        public int TotalChannelUsers;
        public string ChannelTopic;
    }
    internal sealed class LISTResult : ChatResultBase
    {
        public string UserIRCPrefix;
        public List<LISTDataModel> ChannelInfos { get; set; }
        public LISTResult()
        {
            ChannelInfos = new List<LISTDataModel>();
        }
    }
}
