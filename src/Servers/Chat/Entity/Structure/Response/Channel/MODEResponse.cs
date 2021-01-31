using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response
{
    //TODO apply channel abstraction into this class
    internal sealed class MODEResponse : ChatResponseBase
    {
        public MODEResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        private new MODERequest _request => (MODERequest)base._request;
        private new MODEResult _result => (MODEResult)base._result;
        protected override void BuildNormalResponse()
        {
            if (_request.RequestType == ModeRequestType.GetChannelModes)
            {
                //channel modes reply
                string cmdParams = $"{_result.ChannelName} {_result.ChannelModes}";
                SendingBuffer = ChatIRCReplyBuilder.Build(ChatReplyName.MODE, cmdParams);
            }
            else
            {
                //user mode reply
                string cmdParams = $"{_result.JoinerNickName} {_result.ChannelName} {_result.ChannelModes}";
                SendingBuffer = ChatIRCReplyBuilder.Build(ChatReplyName.ChannelModels, cmdParams);
            }
        }
    }
}
