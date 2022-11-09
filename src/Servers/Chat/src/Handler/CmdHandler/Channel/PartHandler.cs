using System.Linq;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.Channel;
using UniSpyServer.Servers.Chat.Entity.Exception.IRC.General;
using UniSpyServer.Servers.Chat.Entity.Structure.Misc.ChannelInfo;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    
    public sealed class PartHandler : ChannelHandlerBase
    {
        private new PartRequest _request => (PartRequest)base._request;
        private new PartResponse _response { get => (PartResponse)base._response; set => base._response = value; }
        private new PartResult _result { get => (PartResult)base._result; set => base._result = value; }
        public PartHandler(IClient client, IRequest request) : base(client, request) { }
        private static RedisClient _redisClient;
        static PartHandler()
        {
            _redisClient = new RedisClient();
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
                        JoinHandler.Channels.Remove(_channel.Name);

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
                    _channel.MultiCastExceptSender(_user, _response);
                    // remove serverInfo in Redis
                    if (_user.Info.GameName is not null)
                    {
                        
                            // todo check how can we determine the server by existing info
                            var server = _redisClient.Context.FirstOrDefault(x =>
                                                    x.HostIPAddress == _user.Connection.RemoteIPEndPoint.Address
                                                    && x.GameName == _user.Info.GameName);
                            if (server is not null)
                            {
                                _redisClient.DeleteKeyValue(server);
                            }
                        
                    }
                    break;
            }
        }
    }
}
