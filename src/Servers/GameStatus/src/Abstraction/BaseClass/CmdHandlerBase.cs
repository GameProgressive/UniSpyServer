using UniSpyServer.Servers.GameStatus.Entity;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.GameStatus.Abstraction.BaseClass
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected CmdHandlerBase(ISession session, IRequest request) : base(session, request)
        {
        }
    }
}
