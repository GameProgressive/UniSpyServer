using System.Linq;
using UniSpyServer.Servers.Chat.Abstraction.BaseClass;
using UniSpyServer.Servers.Chat.Entity.Contract;
using UniSpyServer.Servers.Chat.Entity.Structure.Request.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Response.Channel;
using UniSpyServer.Servers.Chat.Entity.Structure.Result.Channel;
using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.Chat.Handler.CmdHandler.Channel
{
    [HandlerContract("PART")]
    public sealed class PartHandler : ChannelHandlerBase
    {
        private new PartRequest _request => (PartRequest)base._request;
        private new PartResponse _response { get => (PartResponse)base._response; set => base._response = value; }
        private new PartResult _result { get => (PartResult)base._result; set => base._result = value; }
        public PartHandler(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void DataOperation()
        {
            _result = new PartResult();
            _result.LeaverIRCPrefix = _user.UserInfo.IRCPrefix;
            _result.ChannelName = _channel.Name;
            if (_channel.IsPeerServer && _user.IsChannelCreator)
            {
                // Parallel.ForEach(_channel.ChannelUsers, (user) =>
                //  {
                //     // KickAllUserAndShutDownChannel(user);
                //     // We create a new KICKHandler to handle KICK operation for us
                //     var kickRequest = new KICKRequest
                //      {
                //          NickName = user.UserInfo.NickName,
                //          ChannelName = _channel.ChannelName,
                //          Reason = "Server Hoster leaves channel"
                //      };
                //      new KICKHandler(_session, kickRequest).Handle();
                //  });
                foreach (var user in _channel.Users.Values)
                {
                    // We create a new KICKHandler to handle KICK operation for us
                    var kickRequest = new KickRequest
                    {
                        KickeeNickName = user.UserInfo.NickName,
                        ChannelName = _channel.Name,
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
            if (_channel.IsPeerServer && _user.IsChannelCreator)
            {
                // we do nothing here, becase we kicked all user
            }
            else
            {
                //broadcast to all user in channel
                _channel.MultiCastExceptSender(_user, _response);

                // remove serverInfo in Redis
                using (var client = new RedisClient())
                {
                    var server = client.Values.Where(x =>
                                            x.HeartBeatIPEndPoint == _user.UserInfo.Session.RemoteIPEndPoint &
                                            x.GameName == _user.UserInfo.GameName)
                                            .FirstOrDefault();
                    if (server != null)
                    {
                        client.DeleteKeyValue(server);
                    }
                }
            }
            // remove channel in ChannelManager
            if (_channel.Users.Count == 0)
            {
                JoinHandler.Channels.Remove(_channel.Name);
            }
        }
    }
}
