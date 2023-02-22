using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class ListResponse : ResponseBase
    {
        private new ListResult _result => (ListResult)base._result;
        public ListResponse(RequestBase request, ResultBase result) : base(request, result){ }
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
