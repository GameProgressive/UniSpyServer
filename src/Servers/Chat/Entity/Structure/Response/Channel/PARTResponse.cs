using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class PARTResponse : ChatResponseBase
    {
        private new PARTResult _result => (PARTResult)base._result;
        public PARTResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = BuildPartReply(
                  _result.LeaverIRCPrefix,
                  ChatReplyName.PART,
                  _result.Message);
        }

        public static string BuildPartReply(string userIRCPrefix, string channelName, string message)
        {
            return ChatIRCReplyBuilder.Build(
                userIRCPrefix,
                ChatReplyName.PART,
                channelName,
                message);
        }
    }
}
