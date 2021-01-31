using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Message;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Message
{
    internal sealed class NOTICEHandler : ChatMsgHandlerBase
    {
        private new NOTICERequest _request => (NOTICERequest)base._request;
        private new NOTICEResult _result
        {
            get => (NOTICEResult)base._result;
            set => base._result = value;
        }
        public NOTICEHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new NOTICEResult();
        }

        protected override void DataOperation()
        {
            _result.UserIRCPrefix = _user.UserInfo.IRCPrefix;
            base.DataOperation();
        }

        protected override void ResponseConstruct()
        {
            _response = new NOTICEResponse(_request, _result);
        }
    }
}



