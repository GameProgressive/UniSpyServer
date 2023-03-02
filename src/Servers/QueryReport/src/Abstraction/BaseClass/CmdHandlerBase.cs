using UniSpy.Server.QueryReport.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.QueryReport.Abstraction.BaseClass
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
