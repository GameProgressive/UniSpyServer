using UniSpyServer.Servers.CDkey.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.CDkey.Application
{
    /// <summary>
    /// A factory that creates instances of servers
    /// </summary>
    public sealed class ServerFactory : ServerFactoryBase
    {
        public new static Server Server  =>  (Server)ServerFactoryBase.Server;
        public ServerFactory() : base()
        {
            ServerName = "CDkey";
        }
    }
}
