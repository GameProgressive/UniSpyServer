using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.General;
using Chat.Network;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class CRYPTResponse : ChatResponseBase
    {
        private new CRYPTResult _result => (CRYPTResult)base._result;
        public CRYPTResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        protected override void BuildNormalResponse()
        {
            var cmdParams = $"* {ChatServer.ClientKey} {ChatServer.ServerKey}";
            SendingBuffer = ChatIRCReplyBuilder.Build(
                ChatReplyName.SecureKey, cmdParams);
        }
    }
}
