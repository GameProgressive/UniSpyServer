using System;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Structure;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceSearchPlayer.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        /// <summary>
        /// Be careful the return of query function should be List type,
        /// the decision formula should use _result.Count==0
        /// </summary>
        protected new Client _client => (Client)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }

        protected override void HandleException(Exception ex)
        {
            if (ex is GPException)
            {
                _client.Session.Send(ex.Message);
            }
            else
            {
                base.HandleException(ex);
            }
        }
    }
}
