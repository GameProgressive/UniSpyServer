using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request.Message;
using Chat.Entity.Structure.Response.Message;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Message
{
    internal sealed class ATMHandler : ChatMsgHandlerBase
    {
        new ATMRequest _request => (ATMRequest)base._request;
        new ATMResult _result
        {
            get => (ATMResult)base._result;
            set => base._result = value;
        }
        public ATMHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new ATMResult();
        }

        protected override void ChannelMessageDataOpration()
        {
            base.ChannelMessageDataOpration();
            _result.UserIRCPrefix = _user.UserInfo.IRCPrefix;
        }
        protected override void UserMessageDataOperation()
        {
            base.UserMessageDataOperation();
            _result.UserIRCPrefix = _reciever.UserInfo.IRCPrefix;
        }
        protected override void ResponseConstruct()
        {
            _response = new ATMResponse(_request, _result);
        }
    }
}
