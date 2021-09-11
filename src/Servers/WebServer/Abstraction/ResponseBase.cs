using UniSpyLib.Abstraction.BaseClass;

namespace WebServer.Abstraction
{
    public abstract class ResponseBase : UniSpyResponseBase
    {
        protected ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}