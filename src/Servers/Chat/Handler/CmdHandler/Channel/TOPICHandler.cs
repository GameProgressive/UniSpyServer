using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure;
using Chat.Entity.Structure.Misc.ChannelInfo;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    public class TOPICHandler : ChatCmdHandlerBase
    {
        protected new TOPICRequest _request => (TOPICRequest)base._request;
        ChatChannel _channel;
        ChatChannelUser _user;
        public TOPICHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            if (!_session.UserInfo.GetJoinedChannelByName(_request.ChannelName, out _channel))
            {
                _errorCode = ChatErrorCode.Parse;
                return;
            }
            if (!_channel.GetChannelUserBySession(_session, out _user))
            {
                _errorCode = ChatErrorCode.Parse;
                return;
            }
        }

        protected override void DataOperation()
        {
            base.DataOperation();
            if (!_user.IsChannelOperator)
            {
                _errorCode = ChatErrorCode.NotChannelOperator;
                return;
            }
        }

        protected override void BuildNormalResponse()
        {
            switch (_request.RequestType)
            {
                case TOPICCmdType.GetChannelTopic:
                    GetChannelTopic();
                    break;
                case TOPICCmdType.SetChannelTopic:
                    SetChannelTopic();
                    break;
            }
        }

        private void GetChannelTopic()
        {
            if (_channel.Property.ChannelTopic == "" || _channel.Property.ChannelTopic == null)
            {
                _sendingBuffer =
                    TOPICResponse.BuildNoTopicReply(
                    _channel.Property.ChannelName);
            }
            else
            {
                _sendingBuffer =
                    TOPICResponse.BuildTopicReply(
                    _channel.Property.ChannelName,
                    _channel.Property.ChannelTopic);
            }
        }

        private void SetChannelTopic()
        {
            _channel.Property.ChannelTopic = _request.ChannelTopic;
            _sendingBuffer =
                TOPICResponse.BuildTopicReply(
                    _channel.Property.ChannelName,
                    _channel.Property.ChannelTopic);
        }

        protected override void Response()
        {
            base.Response();
            if (_sendingBuffer == null || _sendingBuffer == "" || _sendingBuffer.Length < 3)
            {
                return;
            }
            switch (_request.RequestType)
            {
                case TOPICCmdType.GetChannelTopic:
                    _session.SendAsync(_sendingBuffer);
                    break;
                case TOPICCmdType.SetChannelTopic:
                    _channel.MultiCast(_sendingBuffer);
                    break;
            }

        }

        protected override void ResponseConstruct()
        {
            _response = new TOPICResponse
        }
    }
}
