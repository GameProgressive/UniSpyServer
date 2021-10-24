using UniSpyServer.Chat.Abstraction.BaseClass;
using UniSpyServer.Chat.Entity.Contract;
using UniSpyServer.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Chat.Entity.Structure.Response.Message;
using UniSpyServer.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Chat.Handler.CmdHandler.Message
{
    [HandlerContract("PRIVMSG")]
    public sealed class PrivateMsgHandler : MsgHandlerBase
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
