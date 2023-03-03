using UniSpy.Server.Chat.Abstraction.BaseClass;
using UniSpy.Server.Chat.Application;
using UniSpy.Server.Chat.Exception.IRC.Channel;
using UniSpy.Server.Chat.Exception.IRC.General;
using UniSpy.Server.Chat.Aggregate.Misc.ChannelInfo;
using UniSpy.Server.Chat.Contract.Request.Channel;
using UniSpy.Server.Chat.Contract.Response.Channel;
using UniSpy.Server.Chat.Contract.Result.Channel;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Chat.Handler.CmdHandler.Channel
{

    public sealed class PartHandler : ChannelHandlerBase
    {
        private new PartRequest _request => (PartRequest)base._request;
        private new PartResponse _response { get => (PartResponse)base._response; set => base._response = value; }
        private new PartResult _result { get => (PartResult)base._result; set => base._result = value; }
        public PartHandler(IClient client, IRequest request) : base(client, request) { }
        static PartHandler()
        {
        }
        protected override void RequestCheck()
        {
            if (_request.RawRequest is null)
            {
                _channel = _client.Info.GetJoinedChannel(_request.ChannelName);
                if (_channel is null)
                {
                    throw new ChatIRCNoSuchChannelException($"No such channel {_request.ChannelName}", _request.ChannelName);
                }
                _user = _channel.GetChannelUser(_client);
                if (_user is null)
                {
                    throw new ChatIRCNoSuchNickException($"Can not find user with nickname: {_client.Info.NickName} username: {_client.Info.UserName}");
                }
                return;
            }
            base.RequestCheck();
        }
        protected override void DataOperation()
        {
            _result = new PartResult();
            _result.LeaverIRCPrefix = _user.Info.IRCPrefix;
            _result.ChannelName = _channel.Name;
            switch (_channel.RoomType)
            {
                case PeerRoomType.Normal:
                case PeerRoomType.Staging:
                    if (_user.IsChannelCreator)
                    {
                        foreach (var user in _channel.Users.Values)
                        {
                            // we do not need to send part message to leaver
                            if (user.Info.NickName == _user.Info.NickName)
                            {
                                continue;
                            }
                            // We create a new KICKHandler to handle KICK operation for us
                            var kickRequest = new KickRequest
                            {
                                KickeeNickName = user.Info.NickName,
                                ChannelName = _channel.Name,
                                Reason = _request.Reason,
                            };
                            new KickHandler(_client, kickRequest).Handle();
                        }
                        JoinHandler.Channels.TryRemove(_channel.Name, out _);

                    }
                    goto default;
                default:
                    // we need always remove the connection in leaver and channel
                    _channel.RemoveBindOnUserAndChannel(_user);
                    break;
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new PartResponse(_request, _result);
        }

        protected override void Response()
        {
            switch (_channel.RoomType)
            {
                case PeerRoomType.Normal:
                case PeerRoomType.Staging:
                    // we already kicked all user and remove the channel
                    break;
                default:
                    //broadcast to all user in channel
                    _channel.MultiCast(_user.ClientRef, _response, true);
                    // remove serverInfo in Redis
                    if (_user.Info.GameName is not null)
                    {
                        StorageOperation.Persistance.DeleteGameServerInfo(_user.Connection.RemoteIPEndPoint.Address,
                                                                          _user.Info.GameName);
                    }
                    break;
            }
        }
    }
}
