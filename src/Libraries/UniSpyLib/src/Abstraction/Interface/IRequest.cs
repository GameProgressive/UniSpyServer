
namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IRequest
    {
        object CommandName { get; }
        object RawRequest { get; }
        void Parse();
    }
}
