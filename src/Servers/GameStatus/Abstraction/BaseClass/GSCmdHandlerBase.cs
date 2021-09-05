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
    internal abstract class GSCmdHandlerBase : UniSpyCmdHandlerBase
    {
        protected new GSSession _session => (GSSession)base._session;
        protected new GSRequestBase _request => (GSRequestBase)base._request;
        protected new GSResultBase _result
        {
            get => (GSResultBase)base._result;
            set => base._result = value;
        }
        protected GSCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new GSDefaultResult();
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
            _response = new GSDefaultResponse(_request, _result);
        }
    }
}
