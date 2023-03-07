using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Aggregate.Misc;
using UniSpy.Server.Chat.Contract.Request.General;
using UniSpy.Server.Chat.Contract.Result.General;

namespace UniSpy.Server.Chat.Contract.Response.General
{
    public sealed class GetKeyResponse : ResponseBase
    {
        private new GetKeyResult _result => (GetKeyResult)base._result;
        private new GetKeyRequest _request => (GetKeyRequest)base._request;
        public GetKeyResponse(RequestBase request, ResultBase result) : base(request, result){ }

        public override void Build()
        {
            SendingBuffer = "";
            foreach (var value in _result.Values)
            {
                string cmdParams1 = $"* {_result.NickName} {_request.Cookie} {value}";
                SendingBuffer += IRCReplyBuilder.Build(ResponseName.GetKey, cmdParams1);
            }
            string cmdParams2 = $"* {_request.Cookie} *";
            string tailing = "End of GETKEY.";
            SendingBuffer += IRCReplyBuilder.Build(ResponseName.EndGetKey, cmdParams2, tailing);
        }
    }
}
