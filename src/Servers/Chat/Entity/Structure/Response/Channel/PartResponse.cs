using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class PartResponse : ResponseBase
    {
        private new PartResult _result => (PartResult)base._result;
        public PartResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = BuildPartReply(
                  _result.LeaverIRCPrefix,
                  _result.ChannelName,
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
