using UniSpyServer.Servers.ServerBrowser.Application;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.ServerBrowser.V2.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected QueryReport.V2.Entity.Structure.Redis.GameServer.RedisClient _gameServerRedisClient { get; private set; }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
            _gameServerRedisClient = new QueryReport.V2.Entity.Structure.Redis.GameServer.RedisClient();
        }
    }
}
