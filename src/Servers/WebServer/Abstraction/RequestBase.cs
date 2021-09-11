using UniSpyLib.Abstraction.BaseClass;

namespace WebServer.Abstraction
{
    public abstract class RequestBase : UniSpyRequestBase
    {
        protected RequestBase(object rawRequest) : base(rawRequest)
        {
        }
    }
}