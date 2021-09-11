using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class PingResponse : ResponseBase
    {
        private new PingResult _result => (PingResult)base._result;
        public PingResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.RequesterIRCPrefix, ChatReplyName.PONG, null, null);
        }
    }
}
