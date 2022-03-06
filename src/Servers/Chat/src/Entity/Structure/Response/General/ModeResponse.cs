using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.General
{
    //TODO check BuildChannelModesReply function
    public sealed class ModeResponse : ResponseBase
    {
        private new ModeRequest _request => (ModeRequest)base._request;
        private new ModeResult _result => (ModeResult)base._result;
        public ModeResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result){ }

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
