using UniSpy.Server.QueryReport.V2.Application;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.QueryReport.Application;

namespace UniSpy.Server.QueryReport.V2.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        protected new RequestBase _request => (RequestBase)base._request;
        protected new ResultBase _result { get => (ResultBase)base._result; set => base._result = value; }
        protected CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}
