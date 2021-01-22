using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request.Channel;
using Chat.Entity.Structure.Result;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class GETCHANKEYResponse : ChatChannelResponseBase
    {
        private new GETCHANKEYResult _result => (GETCHANKEYResult)base._result;
        private new GETCHANKEYRequest _request => (GETCHANKEYRequest)base._request;
        public GETCHANKEYResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        protected override void BuildNormalResponse()
        {
            SendingBuffer = _result.ChannelUser.BuildReply(
            ChatReplyName.GetChanKey,
                $"param1 {_result.ChannelName} {_request.Cookie} {_result.Values}");
        }
    }
}
