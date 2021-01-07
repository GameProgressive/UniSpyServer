using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Message
{
    public class ATMResponse : ChatResponseBase
    {
        protected new ATMResult _result
        {
            get { return (ATMResult)base._result; }
        }
        public ATMResponse(UniSpyResultBase result) : base(result)
        {
        }

        public override void Build()
        {
            SendingBuffer =  _result.UserInfo.BuildReply(
                ChatReplyCode.ATM,
                $"{_result.Name} {_result.Message}");
        }
    }
}
