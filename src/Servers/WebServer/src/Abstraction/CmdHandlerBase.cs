using UniSpyServer.Servers.WebServer.Application;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.Servers.WebServer.Abstraction
{
    public abstract class CmdHandlerBase : UniSpyLib.Abstraction.BaseClass.CmdHandlerBase
    {
        protected new Client _client => (Client)base._client;
        protected CmdHandlerBase(IClient client, IRequest request) : base(client, request)
        {
        }
    }
}
