using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;

namespace UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel
{
    public sealed class SetChannelKeyResponse : ChannelResponseBase
    {
        private new SetChannelKeyRequest _request => (SetChannelKeyRequest)base._request;
        private new SetChannelKeyResult _result => (SetChannelKeyResult)base._result;
        public SetChannelKeyResponse(UniSpyLib.Abstraction.BaseClass.RequestBase request, UniSpyLib.Abstraction.BaseClass.ResultBase result) : base(request, result){ }

        public override void Build()
        {
            string flags = "";
            foreach (var kv in _request.KeyValue)
            {
                flags += $@"\{kv.Key}\{kv.Value}";
            }
            var cmdParams = $"param1 {_request.ChannelName} BCAST {flags}";
            SendingBuffer =
                IRCReplyBuilder.Build(
                    _result.ChannelUserIRCPrefix,
                    ResponseName.GetChanKey,
                    cmdParams,
                    null);
        }
    }
}