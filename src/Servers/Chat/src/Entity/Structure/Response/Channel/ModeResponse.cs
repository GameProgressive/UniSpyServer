using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel
{
    //TODO apply channel abstraction into this class
    public sealed class ModeResponse : ResponseBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result => (ModeResult)base._result;
        public ModeResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
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
