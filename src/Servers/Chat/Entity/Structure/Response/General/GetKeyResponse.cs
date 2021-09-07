using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Channel;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Channel
{
    internal sealed class GetKeyResponse : ResponseBase
    {
        private new GETKEYResult _result => (GETKEYResult)base._result;
        private new GetKeyRequest _request => (GetKeyRequest)base._request;
        public GetKeyResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }

        public override void Build()
        {
            SendingBuffer = "";
            foreach (var flag in _result.Flags)
            {
                string cmdParams1 = $"param1 {_result.NickName} {_request.Cookie} {flag}";
                SendingBuffer += ChatIRCReplyBuilder.Build(ChatReplyName.GetKey, cmdParams1);
            }
            string cmdParams2 = $"param1 param2 {_request.Cookie} param4";
            string tailing = "End of GETKEY.";
            SendingBuffer += ChatIRCReplyBuilder.Build(ChatReplyName.EndGetKey, cmdParams2, tailing);
        }
    }
}
