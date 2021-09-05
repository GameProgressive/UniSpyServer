using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class PINGResponse : ChatResponseBase
    {
        private new PINGResult _result => (PINGResult)base._result;
        public PINGResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.RequesterIRCPrefix, ChatReplyName.PONG, null, null);
        }
    }
}
