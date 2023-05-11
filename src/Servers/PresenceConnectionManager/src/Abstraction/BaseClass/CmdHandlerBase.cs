using UniSpy.Server.PresenceConnectionManager.Application;
using UniSpy.Server.PresenceSearchPlayer.Exception.General;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.PresenceConnectionManager.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
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

        protected override void HandleException(System.Exception ex)
        {
            if (ex is GPException)
            {
                _client.Send(ex as IResponse);
            }
            base.HandleException(ex);
        }
    }
}
