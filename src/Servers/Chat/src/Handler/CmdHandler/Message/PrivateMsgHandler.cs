using UniSpyServer.Servers.Chat.Abstraction.BaseClass;

using UniSpyServer.Servers.Chat.Entity.Structure.Request.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Message;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Message;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Message
{
    
    public sealed class PrivateMsgHandler : MsgHandlerBase
    {
        private new PrivateMsgRequest _request => (PrivateMsgRequest)base._request;
        private new PrivateMsgResult _result { get => (PrivateMsgResult)base._result; set => base._result = value; }
        public PrivateMsgHandler(IClient client, IRequest request) : base(client, request)
        {
            _result = new PrivateMsgResult();
        }
        protected override void DataOperation()
        {
            base.DataOperation();
            _result.UserIRCPrefix = _user.Info.IRCPrefix;
        }
        protected override void UserMessageDataOperation()
        {
            _result.TargetName = _reciever.Info.NickName;
        }
        protected override void ChannelMessageDataOpration()
        {
            if (!_channel.Mode.IsModeratedChannel)
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
            if (_user.Info.IsQuietMode)
            {
                return;
            }
            _result.IsBroadcastMessage = true;
            _result.TargetName = _channel.Name;
        }
        protected override void ResponseConstruct()
        {
            _response = new PrivateMsgResponse(_request, _result);
        }
    }
}
