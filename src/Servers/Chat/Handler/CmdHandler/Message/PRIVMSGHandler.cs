using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Message
{
    internal sealed class PRIVMSGHandler : ChatMsgHandlerBase
    {
        new PRIVMSGRequest _request { get { return (PRIVMSGRequest)base._request; } }
        public PRIVMSGHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            switch (_request.RequestType)
            {
                case ChatMessageType.ChannelMessage:
                    BuildChannelMessage();
                    break;
                case ChatMessageType.UserMessage:
                    BuildUserMessage();
                    break;
            }
        }

        private void BuildUserMessage()
        {
            _sendingBuffer =
                PRIVMSGReply.BuildPrivMsgReply(_session.UserInfo, _request.NickName, _request.Message);
        }

        private void BuildChannelMessage()
        {
            if (!_channel.Property.ChannelMode.IsModeratedChannel)
            {
                return;
            }

            if (_channel.IsUserBanned(_user))
            {
                return;
            }

            if (!_user.IsVoiceable)
            {
                return;
            }
            if (_user.UserInfo.IsQuietMode)
            {
                return;
            }

            _sendingBuffer =
               PRIVMSGReply.BuildPrivMsgReply(_user.UserInfo,
               _request.ChannelName, _request.Message);
        }
    }
}
