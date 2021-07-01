using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response
{
    internal sealed class SETCHANKEYResponse : ChatChannelResponseBase
    {
        private new SETCHANKEYRequest _request => (SETCHANKEYRequest)base._request;
        private new SETCHANKEYResult _result => (SETCHANKEYResult)base._result;
        public SETCHANKEYResponse(UniSpyRequest request, UniSpyResult result) : base(request, result)
        {
        }

        public override void Build()
        {
            string flags = "";
            foreach (var kv in _request.KeyValue)
            {
                flags += $@"\{kv.Key}\{kv.Value}";
            }
            var cmdParams = $"param1 {_result.ChannelName} BCAST {flags}";
            SendingBuffer =
                ChatIRCReplyBuilder.Build(
                    _result.ChannelUserIRCPrefix,
                    ChatReplyName.GetChanKey,
                    cmdParams,
                    null);
        }
    }
}