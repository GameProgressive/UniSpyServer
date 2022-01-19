using UniSpyServer.Servers.QueryReport.Entity.Structure.Redis.GameServer;
using UniSpyServer.Servers.QueryReport.Network;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.QueryReport.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected RedisClient _redisClient { get; private set; }
        protected CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _redisClient = new RedisClient();
        }
    }
}
