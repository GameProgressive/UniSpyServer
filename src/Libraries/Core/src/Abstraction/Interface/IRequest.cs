
namespace UniSpy.Server.Core.Abstraction.Interface
{
    public interface IRequest
    {
        object CommandName { get; }
        object RawRequest { get; }
        void Parse();
    }
}
