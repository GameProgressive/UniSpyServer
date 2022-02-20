using UniSpyServer.Servers.QueryReport.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpyLib.Abstraction.BaseClass.ResultBase
    {
        public PacketType? PacketType { get; protected set; }
        public ResultBase()
        {
        }
    }
}