using UniSpyServer.UniSpyLib.Abstraction.BaseClass.Factory;

namespace UniSpyServer.Servers.CDKey.Application
{
    /// <summary>
    /// A factory that creates instances of servers
    /// </summary>
    public sealed class ServerFactory : ServerFactoryBase
    {
        public ServerFactory() : base()
        {
            ServerName = "CDKey";
        }
    }
}
