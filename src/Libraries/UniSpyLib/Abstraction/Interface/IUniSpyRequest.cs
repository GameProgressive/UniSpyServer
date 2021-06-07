
namespace UniSpyLib.Abstraction.Interface
{
    interface IUniSpyRequest
    {
        object CommandName { get; }
        object RawRequest { get; }
        void Parse();
    }
}
