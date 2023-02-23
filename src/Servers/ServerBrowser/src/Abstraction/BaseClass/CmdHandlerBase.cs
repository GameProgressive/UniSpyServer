using UniSpy.Server.ServerBrowser.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.ServerBrowser.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected QueryReport.V2.Aggregate.Redis.GameServer.RedisClient _gameServerRedisClient { get; private set; }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
            _gameServerRedisClient = new QueryReport.V2.Aggregate.Redis.GameServer.RedisClient();
        }
    }
}
