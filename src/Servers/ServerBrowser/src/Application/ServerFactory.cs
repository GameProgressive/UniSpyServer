using UniSpyServer.Servers.ServerBrowser.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;
using UniSpyServer.UniSpyLib.Config;
using UniSpyServer.Servers.QueryReport.Handler.SystemHandler;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyServer.Servers.ServerBrowser.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public sealed class ServerFactory : ServerFactoryBase
    {
        public new static Server Server  =>  (Server)ServerFactoryBase.Server;
        public ServerFactory(): base()
        {
            ServerName = "ServerBrowser";
        }
    }
}
