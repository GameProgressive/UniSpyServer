using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Misc;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Response.General;
using Chat.Handler.SystemHandler.ChannelManage;
using Chat.Handler.SystemHandler.ChatSessionManage;
using Chat.Network;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.General
{
    /// <summary>
    /// Get a channel user's basic information
    /// same as WHOIS
    /// </summary>
    public class WHOHandler : ChatLogedInHandlerBase
    {
        new WHORequest _request { get { return (WHORequest)base._request; } }
        ChatChannel _resultChannel;
        ChatSession _resultSession;
        public WHOHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            switch (_request.RequestType)
            {
                case WHOType.GetChannelUsersInfo:
                    GetChannelUsersInfo();
                    break;
                case WHOType.GetUserInfo:
                    GetUserInfo();
                    break;
            }
        }

        private void GetChannelUsersInfo()
        {
            ChatChannel channel;
            if (!ChatChannelManager.GetChannel(_request.ChannelName, out channel))
            {
                _errorCode = ChatErrorCode.NoSuchChannel;
                return;
            }
            _resultChannel = channel;

        }
        /// <summary>
        /// Send all channel user information
        /// </summary>
        private void GetUserInfo()
        {
            ChatSession session;
            if (!ChatSessionManager.GetSessionByUserName(_request.NickName, out session))
            {
                _errorCode = ChatErrorCode.NoSuchNick;
                return;
            }
            _resultSession = session;
        }

        protected override void BuildNormalResponse()
        {
            base.BuildNormalResponse();
            _sendingBuffer = "";
            switch (_request.RequestType)
            {
                case WHOType.GetChannelUsersInfo:
                    BuildWhoReplyForChannelUser();
                    break;

                case WHOType.GetUserInfo:
                    BuildWhoReplyForUser();
                    break;
            }
        }


        protected override void BuildErrorResponse()
        {
            base.BuildErrorResponse();
            switch (_errorCode)
            {
                case ChatErrorCode.NoSuchChannel:
                    _sendingBuffer =
                        ChatIRCErrorCode.BuildNoSuchChannelError(_request.ChannelName);
                    break;
                case ChatErrorCode.NoSuchNick:
                    _sendingBuffer = ChatIRCErrorCode.BuildNoSuchNickError();
                    break;
            }

        }

        private void BuildWhoReplyForChannelUser()
        {
            foreach (var user in _resultChannel.Property.ChannelUsers)
            {
                _sendingBuffer +=
                    WHOReply.BuildWhoReply(
                        _resultChannel.Property.ChannelName,
                        user.UserInfo, user.GetUserModes());
            }
            _sendingBuffer +=
                WHOReply.BuildEndOfWhoReply(_resultChannel.Property.ChannelName);
        }
        private void BuildWhoReplyForUser()
        {
            foreach (var channel in _resultSession.UserInfo.JoinedChannels)
            {
                ChatChannelUser user;
                channel.GetChannelUserBySession(_resultSession, out user);
                _sendingBuffer +=
                    WHOReply.BuildWhoReply(
                        channel.Property.ChannelName,
                        _resultSession.UserInfo,
                        user.GetUserModes());
            }
            _sendingBuffer +=
                WHOReply.BuildEndOfWhoReply(_resultSession.UserInfo.NickName);
        }
    }
}
