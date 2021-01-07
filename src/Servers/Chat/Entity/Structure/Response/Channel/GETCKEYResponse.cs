using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Result;

namespace Chat.Entity.Structure.Response.General
{
    public class GETCKEYResponse : ChatResponseBase
    {
        protected new GETCKEYResult _result
        {
            get { return (GETCKEYResult)base._result; }
        }

        public GETCKEYResponse(GETCKEYResult result) : base(result)
        {
        }

        public override void Build()
        {
            SendingBuffer = "";
            foreach (var kv in _result.NickNamesAndBFlags)
            {
                SendingBuffer += ChatReplyBuilder.Build(ChatReplyCode.GetCKey,
                $"* {kv.Key} {_result.ChannelName} {_result.Cookie} {kv.Value}");
            }

            SendingBuffer += ChatReplyBuilder.Build(ChatReplyCode.EndGetCKey,
                 $"* {_result.ChannelName} {_result.Cookie}",
                 "End Of /GETCKEY.");
        }
    }
}
