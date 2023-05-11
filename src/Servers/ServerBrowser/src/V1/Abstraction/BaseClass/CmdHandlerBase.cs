using UniSpy.Server.ServerBrowser.V1.Application;

namespace UniSpy.Server.ServerBrowser.V1.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        public CmdHandlerBase(Client client, RequestBase request) : base(client, request)
        {
        }
        protected override void HandleException(System.Exception ex)
        {
            if (ex is ServerBrowser.Exception)
            {
                _client.Send((ServerBrowser.Exception)ex);
            }
            base.HandleException(ex);
        }
    }
}