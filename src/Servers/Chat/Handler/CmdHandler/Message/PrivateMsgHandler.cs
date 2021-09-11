using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Message;
using Chat.Entity.Structure.Result.Message;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Message
{
    [HandlerContract("PRIVMSG")]
    internal sealed class PrivateMsgHandler : MsgHandlerBase
    {
        private new PrivateMsgRequest _request => (PrivateMsgRequest)base._request;
        private new PrivateMsgResult _result { get => (PrivateMsgResult)base._result; set => base._result = value; }
        public PrivateMsgHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new PrivateMsgResult();
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            _result.UserIRCPrefix = _user.UserInfo.IRCPrefix;
        }
        protected override void UserMessageDataOperation()
        {
            _result.TargetName = _reciever.UserInfo.NickName;
        }
        protected override void ChannelMessageDataOpration()
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
            _result.IsBroadcastMessage = true;
            _result.TargetName = _channel.Property.ChannelName;
        }
        protected override void ResponseConstruct()
        {
            _response = new PrivateMsgResponse(_request, _result);
        }
    }
}
