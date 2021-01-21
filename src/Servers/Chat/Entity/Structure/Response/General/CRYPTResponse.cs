using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class CRYPTResponse : ChatResponseBase
    {
        public CRYPTResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        private new CRYPTResult _result => (CRYPTResult)base._result;

        protected override void BuildNormalResponse()
        {
            var cmdParams = $"* {_result.ClientKey} {_result.ServerKey}";
            SendingBuffer = ChatIRCReplyBuilder.Build(
                ChatReplyName.SecureKey, cmdParams);
        }
    }
}
