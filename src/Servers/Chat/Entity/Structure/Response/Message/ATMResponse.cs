using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Message
{
    internal sealed class ATMResponse : ChatResponseBase
    {
        private new ATMResult _result => (ATMResult)base._result;
        public ATMResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }



        public override void Build()
        {
            SendingBuffer = _result.UserInfo.BuildReply(
                ChatReplyName.ATM,
                $"{_result.Name} {_result.Message}");
        }

        protected override void BuildNormalResponse()
        {
            throw new System.NotImplementedException();
        }
    }
}
