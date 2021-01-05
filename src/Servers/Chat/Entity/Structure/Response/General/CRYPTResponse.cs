using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Result;

namespace Chat.Entity.Structure.Response.General
{
    public class CRYPTResponse : ChatResponseBase
    {
        protected new CRYPTResult _result
        {
            get { return (CRYPTResult)base._result; }
        }
        public CRYPTResponse(ChatResultBase result) : base(result)
        {
        }

        public override void Build()
        {
            SendingBuffer = ChatReplyBuilder.Build(
                ChatReplyCode.SecureKey,
                $"* {_result.ClientKey} {_result.ServerKey}");
        }
    }
}
