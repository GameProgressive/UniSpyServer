using PresenceSearchPlayer.Entity.Enumerate;
using PresenceSearchPlayer.Entity.Exception.General;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using PresenceSearchPlayer.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class PSPCmdHandlerBase : UniSpyCmdHandlerBase
    {
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected new PSPRequestBase _request => (PSPRequestBase)base._request;
        protected new PSPSession _session => (PSPSession)base._session;
        protected new PSPResultBase _result
        {
            get => (PSPResultBase)base._result;
            set => base._result = value;
        }

        public PSPCmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
            _result = new PSPDefaultResult();
        }

        public override void Handle()
        {
            try
            {
                RequestCheck();
                DataOperation();
                ResponseConstruct();
                Response();
            }
            catch (GPException e)
            {
                _session.SendAsync(e.ErrorResponse);
            }
        }
        protected override void RequestCheck() { }

        protected override void ResponseConstruct()
        {
            _response = new PSPDefaultResponse(_request, _result);
        }
    }
}
