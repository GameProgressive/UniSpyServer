using UniSpy.Server.QueryReport.Enumerate;

namespace UniSpy.Server.QueryReport.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpy.Server.Core.Abstraction.BaseClass.ResultBase
    {
        public PacketType? PacketType { get; protected set; }
        public ResultBase()
        {
        }
    }
}