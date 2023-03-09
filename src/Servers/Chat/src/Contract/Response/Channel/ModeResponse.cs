using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    //TODO apply channel abstraction into this class
    public sealed class ModeResponse : ResponseBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result => (ModeResult)base._result;
        public ModeResponse(ModeRequest request, ModeResult result) : base(request, result) { }
        public override void Build()
        {
            if (_request.RequestType == ModeRequestType.GetChannelModes)
            {
                //channel modes reply
                SendingBuffer = $":{ServerDomain} {ResponseName.ChannelModels} * {_result.ChannelName} {_result.ChannelModes}\r\n";
            }
            else
            {
                //channel user mode reply
                // the client will know which mode its ask for
                SendingBuffer = $":{ServerDomain} {ResponseName.Mode} * {_result.ChannelName} {_result.ChannelModes}\r\n";
            }
        }
    }
}
