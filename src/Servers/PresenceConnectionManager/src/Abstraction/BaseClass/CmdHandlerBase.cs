using System;
using PresenceSearchPlayer.Entity.Exception.General;
using PresenceConnectionManager.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyCmdHandlerBase
    {
        /// <summary>
        /// Because all errors are sent by SendGPError()
        /// so we if the error code != noerror we send it
        /// </summary>
        protected new Session _session => (Session)base._session;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IUniSpySession session, IUniSpyRequest request) : base(session, request)
        {
        }

        protected override void HandleException(Exception ex)
        {
            if (ex is GPException)
            {
                _session.SendAsync(((GPException)ex).ErrorResponse);
            }
            else
            {
                base.HandleException(ex);
            }
        }
    }
}
