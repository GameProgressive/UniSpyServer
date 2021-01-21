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
        public JOINResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.JoinerPrefix, ChatReplyName.JOIN, _request.ChannelName, null);
        }
    }
}
