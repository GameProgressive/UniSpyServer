using System;
using UniSpyServer.Servers.PresenceConnectionManager.Entity.Structure;
using UniSpyServer.Servers.PresenceSearchPlayer.Entity.Exception.General;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        /// <summary>
        /// Because all errors are sent by SendGPError()
        /// so we if the error code != noerror we send it
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
                _client.Send(((GPException)ex));
            }
            else
            {
                base.HandleException(ex);
            }
        }
    }
}
