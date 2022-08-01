using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using System.Collections.Generic;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Result.General
{
    public sealed record QuitDataModel
    {
        public string ChannelName { get; set; }
        public bool IsPeerServer { get; set; }
        public bool IsChannelCreator { get; set; }
        public string LeaveReplySendingBuffer { get; set; }
        public string KickReplySendingBuffer { get; set; }
    }
    
    public sealed class QuitResult : ResultBase
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