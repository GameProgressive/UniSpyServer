using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class ListResponse : ResponseBase
    {
        private new ListResult _result => (ListResult)base._result;
        public ListResponse(RequestBase request, ResultBase result) : base(request, result) { }
        public override void Build()
        {
            SendingBuffer = "";
            foreach (var info in _result.ChannelInfoList)
            {
                SendingBuffer += $":{_result.UserIRCPrefix} {ResponseName.ListStart} * {info.ChannelName} {info.TotalChannelUsers} {info.ChannelTopic}\r\n";
            }
            SendingBuffer += $":{_result.UserIRCPrefix} {ResponseName.ListEnd}\r\n";
        }
    }
}
