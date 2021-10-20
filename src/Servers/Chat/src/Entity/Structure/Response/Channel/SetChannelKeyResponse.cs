using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Structure.Misc;
using UniSpyServer.Chat.Entity.Structure.Request;
using UniSpyServer.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Chat.Entity.Structure.Response
{
    public sealed class SetChannelKeyResponse : ChannelResponseBase
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