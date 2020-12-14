
namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyRequest
    {
        public object CommandName { get; }
        public object RawRequest { get; }
        public object ErrorCode { get; }
        public void Parse();
    }
}
