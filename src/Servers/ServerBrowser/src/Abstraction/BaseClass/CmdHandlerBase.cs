using UniSpyServer.Servers.ServerBrowser.Network;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new Client _client => (Client)base._client;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected QueryReport.Entity.Structure.Redis.PeerGroup.RedisClient _peerGroupRedisClient { get; private set; }
        protected QueryReport.Entity.Structure.Redis.GameServer.RedisClient _gameServerRedisClient { get; private set; }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
            _peerGroupRedisClient = new QueryReport.Entity.Structure.Redis.PeerGroup.RedisClient();
            _gameServerRedisClient = new QueryReport.Entity.Structure.Redis.GameServer.RedisClient();
        }
    }
}
