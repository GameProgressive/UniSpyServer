using System;
using PresenceSearchPlayer.Entity.Exception.General;
using PresenceSearchPlayer.Entity.Structure.Response;
using PresenceSearchPlayer.Entity.Structure.Result;
using PresenceSearchPlayer.Network;
using UniSpyLib.Abstraction.BaseClass;
using UniSpyLib.Abstraction.Interface;

namespace PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyCmdHandlerBase
    {
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
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
                _session.SendAsync(ex.Message);
            }
            else
            {
                base.HandleException(ex);
            }
        }
    }
}
