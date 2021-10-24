using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.Channel
{
    public sealed class KickResponse : ResponseBase
    {
        public KickResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        private new KickResult _result => (KickResult)base._result;
        private new KickRequest _request => (KickRequest)base._request;

        public override void Build()
        {
            var cmdParams = $"{_result.ChannelName} {_result.KickerNickName} {_result.KickeeNickName}";

            SendingBuffer = IRCReplyBuilder.Build(_result.KickerIRCPrefix, ResponseName.Kick, cmdParams, null);
        }
    }
}
