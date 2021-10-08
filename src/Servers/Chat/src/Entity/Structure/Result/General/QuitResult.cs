using Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace Chat.Entity.Structure.Result.General
{
    internal sealed record QuitDataModel
    {
        public string ChannelName { get; set; }
        public bool IsPeerServer { get; set; }
        public bool IsChannelCreator { get; set; }
        public string LeaveReplySendingBuffer { get; set; }
        public string KickReplySendingBuffer { get; set; }
    }
    
    internal sealed class QuitResult : ResultBase
    {
        public string QuiterPrefix { get; set; }
        public List<QuitDataModel> ChannelInfos { get; }
        public string Message { get; set; }

        public QuitResult()
        {
            ChannelInfos = new List<QuitDataModel>();
        }
    }
}