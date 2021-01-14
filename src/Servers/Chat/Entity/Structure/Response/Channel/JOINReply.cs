using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result.Channel;

namespace Chat.Entity.Structure.Response.General
{
    public class JOINResponse : ChatResponseBase
    {
        public JOINResponse(ChatResultBase result) : base(result)
        {
        }

        protected new JOINResult _result
        {
            get { return (JOINResult)base._result; }
        }

        public override void Build()
        {
            SendingBuffer =
                _result.Joiner.BuildReply(ChatReplyCode.JOIN, _result.ChannelName);
        }
    }
}
