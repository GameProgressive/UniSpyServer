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
        public GetKeyResponse(RequestBase request, ResultBase result) : base(request, result) { }

        public override void Build()
        {
            SendingBuffer = "";
            foreach (var value in _result.Values)
            {
                SendingBuffer += $":{ServerDomain} {ResponseName.GetKey} * {_result.NickName} {_request.Cookie} {value}\r\n";
            }
            SendingBuffer += $":{ServerDomain} {ResponseName.EndGetKey} * {_request.Cookie} * :End of GETKEY.\r\n";
        }
    }
}
