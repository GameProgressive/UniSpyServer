using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Abstraction.Interface;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{

    public sealed class KickHandler : ChannelHandlerBase
    {
        private new KickRequest _request => (KickRequest)base._request;
        private new KickResponse _response { get => (KickResponse)base._response; set => base._response = value; }
        private new KickResult _result { get => (KickResult)base._result; set => base._result = value; }
        private ChannelUser _kickee;
        public KickHandler(IChatClient client, KickRequest request) : base(client, request)
        {
            _result = new KickResult();
        }

        protected override void RequestCheck()
        {
            base.RequestCheck();

            if (!_user.IsChannelOperator)
            {
                throw new Chat.Exception("The Kick operation failed, because you are not channel operator.");
            }
            _kickee = _channel.GetUser(_request.KickeeNickName);
            if (_kickee is null)
            {
                throw new Chat.Exception($"Can not find kickee:{_request.KickeeNickName} in channel.");
            }
        }
        protected override void DataOperation()
        {
            _result.ChannelName = _channel.Name;
            _result.KickerNickName = _client.Info.NickName;
            _result.KickerIRCPrefix = _client.Info.IRCPrefix;
            _result.KickeeNickName = _kickee.Info.NickName;
        }
        protected override void ResponseConstruct()
        {
            _response = new KickResponse(_request, _result);
        }

        protected override void Response()
        {
            _channel.MultiCast(_client, _response);
            _channel.RemoveUser(_kickee);
            Aggregate.Channel.UpdateChannelCache(_user);
            Aggregate.Channel.UpdatePeerRoomInfo(_user);
        }
    }
}
