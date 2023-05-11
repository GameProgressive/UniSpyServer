using UniSpy.Server.QueryReport.Application;

namespace UniSpy.Server.QueryReport.V1.Abstraction.BaseClass
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        public CmdHandlerBase(Client client, RequestBase request) : base(client, request)
        {
        }
    }
}