using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Error.IRC.Channel;
using UniSpy.Server.Chat.Error.IRC.General;
using UniSpy.Server.Chat.Aggregate;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Chat.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{

    public sealed class PartHandler : ChannelHandlerBase
    {
        private new PartRequest _request => (PartRequest)base._request;
        private new PartResponse _response { get => (PartResponse)base._response; set => base._response = value; }
        private new PartResult _result { get => (PartResult)base._result; set => base._result = value; }
        public PartHandler(IShareClient client, PartRequest request) : base(client, request) { }
        static PartHandler()
        {
        }
        protected override void RequestCheck()
        {
            if (_request.RawRequest is null)
            {
                _channel = _client.Info.GetLocalJoinedChannel(_request.ChannelName);
                if (_channel is null)
                {
                    throw new NoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
                }
                _user = _channel.GetUser(_client);
                if (_user is null)
                {
                    throw new NoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
                }
            }
            else
            {
                base.RequestCheck();
            }
        }
        protected override void DataOperation()
        {
            _result = new PartResult();
            _result.LeaverIRCPrefix = _user.Client.Info.IRCPrefix;
            _result.ChannelName = _channel.Name;
            switch (_channel.RoomType)
            {
                case PeerRoomType.Normal:
                case PeerRoomType.Staging:
                    if (_user.IsChannelCreator)
                    {
                        switch (_channel.RoomType)
                        {
                            case PeerRoomType.Normal:
                            case PeerRoomType.Staging:
                                Aggregate.Channel.RemoveLocalChannel(_channel);
                                Aggregate.Channel.RemoveChannelCache(_user, _channel);
                                break;
                        }
                        foreach (var user in _channel.Users.Values)
                        {
                            // we do not need to send part message to leaver
                            if (user.Client.Info.NickName == _user.Client.Info.NickName)
                            {
                                continue;
                            }
                            // We create a new KICKHandler to handle KICK operation for us
                            var kickRequest = new KickRequest
                            {
                                KickeeNickName = user.Client.Info.NickName,
                                ChannelName = _channel.Name,
                                Reason = _request.Reason,
                            };
                            new KickHandler(_client, kickRequest).Handle();
                        }
                    }
                    goto default;
                default:
                    // we need always remove the connection in leaver and channel
                    _channel.RemoveUser(_user);
                    Aggregate.Channel.UpdateChannelCache(_user, _channel);
                    // Aggregate.Channel.UpdatePeerRoomInfo(_user);
                    break;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new PartResponse(_request, _result);
        }

        protected override void Response()
        {
            // when user leave channel we must broadcast leave message, whatever the room type is.
            // otherwise the other client will not delete this user in his client list
            _channel.MultiCast(_user.Client, _response, true);
        }
    }
}
