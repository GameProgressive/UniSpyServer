using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class KickResponse : ResponseBase
    {
        private new KickRequest _request => (KickRequest)base._request;
        private new KickResult _result => (KickResult)base._result;
        public KickResponse(KickRequest request, KickResult result) : base(request, result) { }
        public override void Build()
        {
            SendingBuffer = $":{_result.KickerIRCPrefix} {ResponseName.Kick} {_result.ChannelName} {_result.KickeeNickName} :{_request.Reason}\r\n";
        }
    }
}
