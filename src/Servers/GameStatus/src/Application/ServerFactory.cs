using UniSpyServer.Servers.GameStatus.Network;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;
using UniSpyServer.UniSpyLib.Config;

namespace UniSpyServer.Servers.GameStatus.Application
{
    /// <summary>
    /// A factory that creates instances of servers
    /// </summary>
    public sealed class ServerFactory : ServerFactoryBase
    {
        public new static Server Server  =>  (Server)ServerFactoryBase.Server;
        public ServerFactory(): base()
        {
            ServerName = "GameStatus";
        }
    }
}
