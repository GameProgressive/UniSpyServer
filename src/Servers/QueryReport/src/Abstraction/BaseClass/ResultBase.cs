using UniSpyServer.QueryReport.Entity.Enumerate;
using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.QueryReport.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpyResultBase
    {
        public PacketType? PacketType { get; protected set; }
        public ResultBase()
        {
        }
    }
}