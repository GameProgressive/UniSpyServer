using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    //TODO check BuildChannelModesReply function
    public sealed class ModeResponse : ResponseBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result => (ModeResult)base._result;
        public ModeResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public static string BuildChannelModeReply(string channelName, string modes)
        {
            var cmdParams = $"{channelName} {modes}";
            return IRCReplyBuilder.Build(ResponseName.Mode, cmdParams);
        }

        public static string BuildChannelUserModesReply(string nickName, string channelName, string modes)
        {
            return IRCReplyBuilder.Build(ResponseName.ChannelModels,
                $"{nickName} {channelName} {modes}");
        }

        public override void Build()
        {
            // we only broadcast the channel 
            if (_request.RequestType == ModeRequestType.GetChannelModes)
            {
                SendingBuffer = BuildChannelModeReply(_result.ChannelName, _result.ChannelModes);
            }
            else
            {
                SendingBuffer = BuildChannelUserModesReply(_result.JoinerNickName, _result.ChannelName, _result.ChannelModes);
            }
        }
    }
}
