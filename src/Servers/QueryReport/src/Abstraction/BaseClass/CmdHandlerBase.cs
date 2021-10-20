using UniSpyServer.QueryReport.Entity.Structure.Response;
using UniSpyServer.QueryReport.Entity.Structure.Result;
using UniSpyServer.QueryReport.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.QueryReport.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
