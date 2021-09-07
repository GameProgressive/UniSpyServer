using QueryReport.Entity.Enumerate;
using UniSpyLib.Abstraction.BaseClass;

namespace QueryReport.Abstraction.BaseClass
{
    internal abstract class ResultBase : UniSpyResultBase
    {
        public PacketType? PacketType { get; protected set; }
        public ResultBase()
        {
        }
    }
}