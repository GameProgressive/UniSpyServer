using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.General
{
    internal sealed class JOINResponse : ChatResponseBase
    {
        private new JOINResult _result => (JOINResult)base._result;
        private new JOINRequest _request => (JOINRequest)base._request;
        public string SendingBufferOfChannelUsers { get; private set; }
        public JOINResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
             string modeReply = MODEReply.BuildModeReply(
                       _request.ChannelName, _result.ChannelModes);
                       
            if (!_result.IsAlreadyJoinedChannel)
            {
                SendingBufferOfChannelUsers = ChatIRCReplyBuilder.Build(
                    _result.JoinerPrefix,
                    ChatReplyName.JOIN,
                    _request.ChannelName,
                    null);
                SendingBufferOfChannelUsers += modeReply;
            }
            
            SendingBuffer = modeReply;
            //check the message :@<nickname> whether broadcast char @ ?
            SendingBuffer += NAMEReply.BuildNameReply(
                _result.JoinerNickName,
                _request.ChannelName,
                _result.ChannelUserNicks);

            SendingBuffer += NAMEReply.BuildEndOfNameReply(
                _result.JoinerNickName,
                 _request.ChannelName);

            SendingBuffer = ChatIRCReplyBuilder.Build(
                _result.JoinerPrefix,
                ChatReplyName.JOIN,
                _request.ChannelName,
                 null);
        }
    }
}
