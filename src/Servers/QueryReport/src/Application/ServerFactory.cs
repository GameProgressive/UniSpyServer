using UniSpyServer.Servers.QueryReport.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyServer.Servers.QueryReport.Application
{
    /// <summary>
    /// A factory that create the instance of servers
    /// </summary>
    public sealed class ServerFactory : ServerFactoryBase
    {
        public new static Server Server  =>  (Server)ServerFactoryBase.Server;
        public ServerFactory(): base()
        {
            ServerName = "QueryReport";
        }
    }
}
