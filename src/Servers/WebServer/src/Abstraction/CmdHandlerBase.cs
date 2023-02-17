using UniSpy.Server.WebServer.Application;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.WebServer.Abstraction
{
    public abstract class CmdHandlerBase : UniSpy.Server.Core.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        protected CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}
