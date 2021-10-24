using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response.Channel
{
    public sealed class JoinResponse : ResponseBase
    {
        private new JoinResult _result => (JoinResult)base._result;
        private new JoinRequest _request => (JoinRequest)base._request;
        // public string SendingBufferOfChannelUsers { get; private set; }
        public JoinResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            // string modeReply = MODEResponse.BuildChannelModeReply(
            //           _request.ChannelName, _result.ChannelModes);

            // if (!_result.IsAlreadyJoinedChannel)
            // {
            //     SendingBufferOfChannelUsers = ChatIRCReplyBuilder.Build(
            //         _result.JoinerPrefix,
            //         ChatReplyName.JOIN,
            //         _request.ChannelName,
            //         null);
            //     SendingBufferOfChannelUsers += modeReply;
            // }

            SendingBuffer = IRCReplyBuilder.Build(
                _result.JoinerPrefix,
                ResponseName.Join,
                _request.ChannelName,
                 null);
        }
    }
}
