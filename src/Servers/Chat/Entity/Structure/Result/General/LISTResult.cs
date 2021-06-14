using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

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
        public List<LISTDataModel> ChannelInfoList { get; }
        public LISTResult()
        {
            ChannelInfoList = new List<LISTDataModel>();
        }
    }
}
