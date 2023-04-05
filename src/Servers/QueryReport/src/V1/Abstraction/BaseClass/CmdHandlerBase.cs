using UniSpy.Server.QueryReport.Application;

namespace UniSpy.Server.QueryReport.V1.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        public CmdHandlerBase(Client client, RequestBase request) : base(client, request)
        {
        }
        protected override void HandleException(System.Exception ex)
        {
            if (ex is QueryReport.Exception)
            {
                _client.Send((QueryReport.Exception)ex);
            }
            else
            {
                base.HandleException(ex);
            }
        }
    }
}