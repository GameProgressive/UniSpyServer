using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class JoinResponse : ResponseBase
    {
        private new JoinRequest _request => (JoinRequest)base._request;
        private new JoinResult _result => (JoinResult)base._result;
        // public string SendingBufferOfChannelUsers { get; private set; }
        public JoinResponse(JoinRequest request, JoinResult result) : base(request, result) { }

        public override void Build()
        {
            SendingBuffer = $":{_result.JoinerPrefix} {ResponseName.Join} {_request.ChannelName}\r\n";
        }
    }
}
