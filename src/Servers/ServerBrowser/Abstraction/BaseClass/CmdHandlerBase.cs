using ServerBrowser.Entity.Structure.Response;
using ServerBrowser.Entity.Structure.Result;
using ServerBrowser.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new Session _session => (Session)base._session;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
