using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.BaseClass;

namespace Chat.Entity.Structure.Response.Message
{
    internal sealed class NOTICEResponse : ResponseBase
    {
        private new NOTICEResult _result => (NOTICEResult)base._result;
        private new NoticeRequest _request => (NoticeRequest)base._request;
        public NOTICEResponse(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
        public override void Build()
        {
            SendingBuffer = ChatIRCReplyBuilder.Build(_result.UserIRCPrefix, _result.TargetName, _request.Message);
        }
    }
}
