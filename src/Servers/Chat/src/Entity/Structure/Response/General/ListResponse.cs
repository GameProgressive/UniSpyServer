using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.General;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    public sealed class ListResponse : ResponseBase
    {
        private new ListResult _result => (ListResult)base._result;
        public ListResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result){ }
        public override void Build()
        {
            SendingBuffer = "";
            foreach (var info in _result.ChannelInfoList)
            {
                var cmdParams = $"param1 {info.ChannelName} {info.TotalChannelUsers} {info.ChannelTopic}";
                SendingBuffer += IRCReplyBuilder.Build(
                    _result.UserIRCPrefix,
                    ResponseName.ListStart,
                    cmdParams,
                    null);
            }
            SendingBuffer += IRCReplyBuilder.Build(_result.UserIRCPrefix, ResponseName.ListEnd, null, null);
        }
    }
}
