using PresenceConnectionManager.Entity.Structure.Response;
using PresenceConnectionManager.Entity.Structure.Result;
using PresenceConnectionManager.Network;
using PresenceSearchPlayer.Entity.Exception.General;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;
using UniSpyLib.Extensions;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    /// <summary>
    /// Because all errors are sent by SendGPError()
    /// so we if the error code != noerror we send it
    /// </summary>
    internal abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.UniSpyCmdHandlerBase
    {
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new PCMResultBase _result { get => (PCMResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
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
    }
}
