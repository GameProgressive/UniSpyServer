using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class KICKResponse : ResponseBase
    {
        public KICKResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        private new KICKResult _result => (KICKResult)base._result;
        private new KickRequest _request => (KickRequest)base._request;

        public override void Build()
        {
            var cmdParams = $"{_result.ChannelName} {_result.KickerNickName} {_result.KickeeNickName}";

            SendingBuffer = ChatIRCReplyBuilder.Build(_result.KickerIRCPrefix, ChatReplyName.KICK, cmdParams, null);
        }
    }
}
