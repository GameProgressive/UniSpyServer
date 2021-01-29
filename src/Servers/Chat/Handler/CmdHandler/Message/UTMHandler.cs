using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Message
{
    internal sealed class UTMHandler : ChatMsgHandlerBase
    {
        new UTMRequest _request { get { return (UTMRequest)base._request; } }

        public UTMHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void BuildNormalResponse()
        {
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    _sendingBuffer =
                        UTMReply.BuildUTMReply(_user.UserInfo, _request.ChannelName, _request.Message);
                    break;
                case ChatMessageType.UserMessage:
                    _sendingBuffer =
                        UTMReply.BuildUTMReply(
                        _session.UserInfo, _request.NickName, _request.Message);
                    break;
            }
        }
    }
}