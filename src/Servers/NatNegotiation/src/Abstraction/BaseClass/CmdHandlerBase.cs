using UniSpyServer.Servers.NatNegotiation.Entity.Structure;
using UniSpyServer.Servers.NatNegotiation.Entity.Structure.Redis;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.NatNegotiation.Abstraction.BaseClass
{
    /// <summary>
    /// because we are using self defined error code so we do not need
    /// to send it to client, when we detect errorCode != noerror we just log it
    /// </summary>
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected static RedisClient _redisClient;
        static CmdHandlerBase()
        {
            _redisClient = new RedisClient();
        }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}
