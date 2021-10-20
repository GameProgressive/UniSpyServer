
namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyRequest
    {
        object CommandName { get; }
        object RawRequest { get; }
        void Parse();
    }
}
