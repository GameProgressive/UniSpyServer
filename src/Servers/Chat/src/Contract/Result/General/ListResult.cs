using UniSpy.Server.Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpy.Server.Chat.Contract.Result.General
{
    public sealed class ListDataModel
    {
        public string ChannelName { get; set; }
        public int TotalChannelUsers { get; set; }
        public string ChannelTopic { get; set; }
    }

    public sealed class ListResult : ResultBase
    {
        public string UserIRCPrefix { get; set; }
        public List<ListDataModel> ChannelInfoList { get; }
        public ListResult()
        {
            ChannelInfoList = new List<ListDataModel>();
        }
    }
}
