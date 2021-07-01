using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class LOGINResponse : ChatResponseBase
    {
        private new LOGINResult _result => (LOGINResult)base._result;
        public LOGINResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(
                ChatReplyName.Login,
                cmdParams: $"* {_result.UserID} {_result.ProfileID}");
        }
    }
}
