using GameStatus.Entity.Structure.Response;
using GameStatus.Entity.Structure.Result;
using GameStatus.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace GameStatus.Abstraction.BaseClass
{
    /// <summary>
    /// we only use selfdefine error code here
    /// so we do not need to send it to client
    /// </summary>
    internal abstract class CmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
