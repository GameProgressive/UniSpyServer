using UniSpyServer.Servers.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.QueryReport.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpyResultBase
    {
        public PacketType? PacketType { get; protected set; }
        public ResultBase()
        {
        }
    }
}