
namespace UniSpyLib.Abstraction.Interface
{
    public interface IRequest
    {
        object CommandName { get; }
        object RawRequest { get; }
        object Parse();
    }
}
