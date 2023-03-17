using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;

namespace UniSpy.Server.Chat.Contract.Response.Channel
{
    public sealed class SetChannelKeyResponse : ChannelResponseBase
    {
        private new SetChannelKeyRequest _request => (SetChannelKeyRequest)base._request;
        private new SetChannelKeyResult _result => (SetChannelKeyResult)base._result;
        public SetChannelKeyResponse(SetChannelKeyRequest request, SetChannelKeyResult result) : base(request, result) { }
        
        public override void Build()
        {
            // the broadcast message must contain key and values.
            SendingBuffer = $":{_result.ChannelUserIRCPrefix} {ResponseName.GetChanKey} * {_request.ChannelName} BCAST {_request.KeyValueString}\r\n";
        }
    }
}