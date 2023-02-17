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
        public ModeResponse(UniSpy.Server.Core.Abstraction.BaseClass.RequestBase request, UniSpy.Server.Core.Abstraction.BaseClass.ResultBase result) : base(request, result){ }
        public override void Build()
        {
            if (_request.RequestType == ModeRequestType.GetChannelModes)
            {
                //channel modes reply
                string cmdParams = $"{_result.ChannelName} {_result.ChannelModes}";
                SendingBuffer = IRCReplyBuilder.Build(ResponseName.Mode, cmdParams);
            }
            else if (_request.RequestType == ModeRequestType.GetChannelUserModes)
            {
                //channel user mode reply
                string cmdParams = $"{_result.JoinerNickName} {_result.ChannelName} {_result.ChannelModes}";
                SendingBuffer = IRCReplyBuilder.Build(ResponseName.ChannelModels, cmdParams);
            }
        }
    }
}
