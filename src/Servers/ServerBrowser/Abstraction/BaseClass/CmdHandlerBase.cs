using ServerBrowser.Entity.Structure.Response;
using ServerBrowser.Entity.Structure.Result;
using ServerBrowser.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace ServerBrowser.Abstraction.BaseClass
{
    internal abstract class CmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new RequestBase _request => (RequestBase)base._request;
        protected new Session _session => (Session)base._session;
        protected new SBResultBase _result
        {
            get => (SBResultBase)base._result;
            set => base._result = value;
        }
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new SBDefaultResult();
        }

        public override void Handle()
        {
            RequestCheck();
            DataOperation();
            ResponseConstruct();
            Response();
        }

        protected override void RequestCheck() { }

        protected override void ResponseConstruct()
        {
            _response = new SBDefaultResponse(_request, _result);
        }

    }
}
