using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Result;

namespace Chat.Entity.Structure.Response.Channel
{
    public class GETCHANKEYResponse : ChatResponseBase
    {
        public GETCHANKEYResponse(ChatResultBase result) : base(result)
        {
        }

        protected new GETCHANKEYResult _result
        {
            get { return (GETCHANKEYResult)base._result; }
        }

        public override void Build()
        {
            SendingBuffer = _result.ChannelUser.BuildReply(
                ChatReplyCode.GetChanKey,
                $"param1 {_result.ChannelName} {_result.Cookie} {_result.Flags}");
        }
    }
}
