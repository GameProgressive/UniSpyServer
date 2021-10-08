using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response
{
    internal sealed class SetChannelKeyResponse : ChannelResponseBase
    {
        private new SetChannelKeyRequest _request => (SetChannelKeyRequest)base._request;
        private new SetChannelKeyResult _result => (SetChannelKeyResult)base._result;
        public SetChannelKeyResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
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
                IRCReplyBuilder.Build(
                    _result.ChannelUserIRCPrefix,
                    ResponseName.GetChanKey,
                    cmdParams,
                    null);
        }
    }
}