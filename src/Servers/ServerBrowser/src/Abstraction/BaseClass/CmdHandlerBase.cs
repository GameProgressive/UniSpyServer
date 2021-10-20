using UniSpyServer.ServerBrowser.Entity.Structure.Response;
using UniSpyServer.ServerBrowser.Entity.Structure.Result;
using UniSpyServer.ServerBrowser.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.ServerBrowser.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyServer.UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new Session _session => (Session)base._session;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }
    }
}
