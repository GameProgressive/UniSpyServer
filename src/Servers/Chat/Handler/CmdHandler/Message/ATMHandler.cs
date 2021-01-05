using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.ChatCommand.Message;
using Chat.Entity.Structure.Response.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Message
{
    public class ATMHandler : ChatMsgHandlerBase
    {
        new ATMRequest _request
        {
            get { return (ATMRequest)base._request; }
        }
        public ATMHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer =
                        ATMResponse.BuildATMReply(
                        _user.UserInfo, _request.ChannelName, _request.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer =
                        ATMResponse.BuildATMReply(
                        _session.UserInfo, _request.NickName, _request.Message);
                    break;
            }
        }
    }
}
