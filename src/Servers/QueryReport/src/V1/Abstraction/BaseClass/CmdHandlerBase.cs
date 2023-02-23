using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.QueryReport.Exception;

namespace UniSpy.Server.QueryReport.V1.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        public CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
        protected override void HandleException(System.Exception ex)
        {
            if (ex is QRException)
            {
                _client.Send(ex as IResponse);
            }
            else
            {
                base.HandleException(ex);
            }
        }
    }
}