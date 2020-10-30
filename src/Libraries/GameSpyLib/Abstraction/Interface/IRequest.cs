
namespace GameSpyLib.Abstraction.Interface
{
    public interface IRequest
    {
        public object Parse();
        public object GetInstance();
    }
}
