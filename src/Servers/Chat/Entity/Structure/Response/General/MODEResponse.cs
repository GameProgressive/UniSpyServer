using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    //TODO check BuildChannelModesReply function
    internal sealed class MODEResponse : ChatResponseBase
    {
        private new MODERequest _request => (MODERequest)base._request;
        private new MODEResult _result =>(MODEResult)base._result;
        public MODEResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public static string BuildModeReply(string channelName, string modes)
        {
            return ChatIRCReplyBuilder.Build(ChatReplyName.MODE, $"{channelName} {modes}");
        }

        public static string BuildChannelModesReply(string nickName, string channelName, string modes)
        {
            return ChatIRCReplyBuilder.Build(ChatReplyName.ChannelModels,
                $"{nickName} {channelName} {modes}");
        }

        protected override void BuildNormalResponse()
        {
            // we only broadcast the channel 
            if (_request.RequestType == ModeRequestType.GetChannelModes)
            {
                SendingBuffer = BuildModeReply(_result.ChannelName,_result.ChannelModes);
            }
        }
    }
}
