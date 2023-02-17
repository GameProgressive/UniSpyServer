using UniSpy.Server.QueryReport.V2.Entity.Enumerate;

namespace UniSpy.Server.QueryReport.V2.Abstraction.BaseClass
{
    public abstract class ResultBase : UniSpy.Server.Core.Abstraction.BaseClass.ResultBase
    {
        public PacketType? PacketType { get; protected set; }
        public ResultBase()
        {
        }
    }
}