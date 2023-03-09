using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class NamesResponse : ChannelResponseBase
    {
        private new NamesRequest _request => (NamesRequest)base._request;
        private new NamesResult _result => (NamesResult)base._result;
        public NamesResponse(NamesRequest request, NamesResult result) : base(request, result) { }
        public override void Build()
        {
            SendingBuffer = $":{ServerDomain} {ResponseName.NameReply} {_result.RequesterNickName} = {_result.ChannelName} :{_result.AllChannelUserNicks}\r\n";

            SendingBuffer += $":{ServerDomain} {ResponseName.EndOfNames} {_result.RequesterNickName} {_result.ChannelName} :End of NAMES list.\r\n";

        }
    }
}
