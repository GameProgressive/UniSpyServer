using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response
{
    //TODO apply channel abstraction into this class
    public sealed class ModeResponse : ResponseBase
    {
        public ModeResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result => (ModeResult)base._result;
        public override void Build()
        {
            if (_request.RequestType == ModeRequestType.GetChannelModes)
            {
                //channel modes reply
                string cmdParams = $"{_result.ChannelName} {_result.ChannelModes}";
                SendingBuffer = IRCReplyBuilder.Build(ResponseName.Mode, cmdParams);
            }
            else
            {
                //channel user mode reply
                string cmdParams = $"{_result.JoinerNickName} {_result.ChannelName} {_result.ChannelModes}";
                SendingBuffer = IRCReplyBuilder.Build(ResponseName.ChannelModels, cmdParams);
            }
        }
    }
}
