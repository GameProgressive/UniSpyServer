using UniSpyServer.NatNegotiation.Application;
using UniSpyServer.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.NatNegotiation.Entity.Structure.Response;
using UniSpyServer.NatNegotiation.Entity.Structure.Result;
using UniSpyServer.NatNegotiation.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;
using UniSpyServer.LinqToRedis;

namespace UniSpyServer.NatNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected RedisClient<NewUserInfo> _redisClient;
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _redisClient = new RedisClient<NewUserInfo>(ServerFactory.Redis, (int)UniSpyServer.UniSpyLib.Extensions.DbNumber.NatNeg);
        }
    }
}
