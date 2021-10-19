using Chat.Abstraction.BaseClass;
using Chat.Entity.Contract;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using Chat.Handler.SystemHandler.ChannelManage;
using QueryReport.Entity.Structure.Redis;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("PART")]
    public sealed class PartHandler : ChannelHandlerBase
    {
        private new PartRequest _request => (PartRequest)base._request;
        private new PartResponse _response
        {
            get => (PartResponse)base._response;
            set => base._response = value;
        }
        private new PartResult _result
        {
            get => (PartResult)base._result;
            set => base._result = value;
        }
        public PartHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new PartResult();
        }

        protected override void DataOperation()
        {
            _result.LeaverIRCPrefix = _user.UserInfo.IRCPrefix;
            _result.ChannelName = _channel.Property.ChannelName;
            if (_channel.Property.IsPeerServer && _user.IsChannelCreator)
            {
                // Parallel.ForEach(_channel.ChannelUsers, (user) =>
                //  {
                //     // KickAllUserAndShutDownChannel(user);
                //     // We create a new KICKHandler to handle KICK operation for us
                //     var kickRequest = new KICKRequest
                //      {
                //          NickName = user.UserInfo.NickName,
                //          ChannelName = _channel.Property.ChannelName,
                //          Reason = "Server Hoster leaves channel"
                //      };
                //      new KICKHandler(_session, kickRequest).Handle();
                //  });
                foreach (var user in _channel.Property.ChannelUsers)
                {
                    // We create a new KICKHandler to handle KICK operation for us
                    var kickRequest = new KickRequest
                    {
                        KickeeNickName = user.UserInfo.NickName,
                        ChannelName = _channel.Property.ChannelName,
                        Reason = _request.Reason
                    };
                    new KickHandler(_session, kickRequest).Handle();
                }

            }
            else
            {
                _channel.RemoveBindOnUserAndChannel(_user);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new PartResponse(_request, _result);
        }

        protected override void Response()
        {
            _response.Build();
            if (_channel.Property.IsPeerServer && _user.IsChannelCreator)
            {
                // we do nothing here, becase we kicked all user
            }
            else
            {
                _session.SendAsync(_response.SendingBuffer);
                // remove serverInfo in Redis
                var searchKey = new GameServerInfoRedisKey()
                {
                    RemoteIPEndPoint = _user.UserInfo.Session.RemoteIPEndPoint,
                    GameName = _user.UserInfo.GameName
                };

                GameServerInfoRedisOperator.DeleteKeyValue(searchKey);
            }
            // remove channel in ChannelManager
            ChatChannelManager.RemoveChannel(_channel);
        }
    }
}
