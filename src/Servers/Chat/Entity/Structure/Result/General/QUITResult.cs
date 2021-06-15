using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed class QUITDataModel
    {
        public string ChannelName { get; set; }
        public bool IsPeerServer { get; set; }
        public bool IsChannelCreator { get; set; }
        public string LeaveReplySendingBuffer { get; set; }
        public string KickReplySendingBuffer { get; set; }
    }
    internal sealed class QUITResult : ChatResultBase
    {
        public string QuiterPrefix { get; set; }
        public List<QUITDataModel> ChannelInfos { get; }
        public string Message { get; set; }

        public QUITResult()
        {
            ChannelInfos = new List<QUITDataModel>();
        }
    }
}