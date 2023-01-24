using UniSpyServer.Servers.QueryReport.V2.Entity.Enumerate;

namespace UniSpyServer.Servers.QueryReport.V2.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpyLib.Abstraction.BaseClass.ResultBase
    {
        public PacketType? PacketType { get; protected set; }
        public ResultBase()
        {
        }
    }
}