using QueryReport.Entity.Structure.Response;
using QueryReport.Entity.Structure.Result;
using QueryReport.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace QueryReport.Abstraction.BaseClass
{
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
