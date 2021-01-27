using System.Threading.Tasks;
using Chat.Abstraction.BaseClass;
using Chat.Entity.Structure.Request;
using Chat.Entity.Structure.Response.Channel;
using Chat.Entity.Structure.Result.Channel;
using Chat.Handler.SystemHandler.ChannelManage;
using QueryReport.Entity.Structure;
using UniSpyLib.Abstraction.Interface;

namespace Chat.Handler.CmdHandler.Channel
{
    internal sealed class PARTHandler : ChatChannelHandlerBase
    {
        private new PARTRequest _request => (PARTRequest)base._request;
        private new PARTResponse _response
        {
            get => (PARTResponse)base._response;
            set => base._response = value;
        }
        private new PARTResult _result
        {
            get => (PARTResult)base._result;
            set => base._result = value;
        }
        public PARTHandler(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void DataOperation()
        {
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
                foreach (var user in _channel.ChannelUsers)
                {
                    // KickAllUserAndShutDownChannel(user);
                    // We create a new KICKHandler to handle KICK operation for us
                    var kickRequest = new KICKRequest
                    {
                        NickName = user.UserInfo.NickName,
                        ChannelName = _channel.Property.ChannelName,
                        Reason = _request.Reason
                    };
                    new KICKHandler(_session, kickRequest).Handle();
                }

            }
            else
            {
                _channel.RemoveBindOnUserAndChannel(_user);
            }
        }

        protected override void ResponseConstruct()
        {
            _response = new PARTResponse(_request, _result);
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
                var fullKey = GameServerInfo.RedisOperator.BuildFullKey(
                    _user.UserInfo.Session.RemoteIPEndPoint,
                    _user.UserInfo.Session.UserInfo.GameName);
                GameServerInfo.RedisOperator.DeleteKeyValue(fullKey);
            }
            // remove channel in ChannelManager
            ChatChannelManager.RemoveChannel(_channel);
        }
    }
}
