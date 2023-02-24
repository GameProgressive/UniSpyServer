using UniSpy.Server.Core.Abstraction.BaseClass;

namespace UniSpy.Server.Core.Abstraction.Interface
{
    public interface IHandler
    {
        void Handle();
    }
    public interface ITestHandler
    {
        RequestBase Request { get; }
        ResultBase Result { get; }
        ResponseBase Response { get; }
    }
}
