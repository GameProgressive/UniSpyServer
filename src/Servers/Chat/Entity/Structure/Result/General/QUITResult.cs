using System.Collections.Generic;
using Chat.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Result.General
{
    internal class QUITDataModel
    {
        public string ChannelName { get; set; }
        public bool IsPeerServer { get; set; }
        public bool IsChannelCreator { get; set; }
        public string LeaveReplySendingBuffer { get; set; }
        public string KickReplySendingBuffer { get; set; }
    }
    internal class QUITResult : ChatResultBase
    {
        public string QuiterPrefix { get; set; }
        public List<QUITDataModel> ChannelInfos { get; set; }
        public string Message { get; set; }

        public QUITResult()
        {
        }
    }
}