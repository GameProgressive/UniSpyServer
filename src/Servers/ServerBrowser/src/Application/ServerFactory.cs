using UniSpyServer.Servers.ServerBrowser.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public sealed class ServerFactory : UniSpyLib.Abstraction.BaseClass.Factory.ServerFactory
    {
        public new static Server Server  =>  (Server)UniSpyLib.Abstraction.BaseClass.Factory.ServerFactory.Server;
        public ServerFactory(): base()
        {
            ServerName = "ServerBrowser";
        }
    }
}
