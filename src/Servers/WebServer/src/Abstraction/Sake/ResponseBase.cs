using UniSpyServer.UniSpyLib.Abstraction.BaseClass;

namespace UniSpyServer.Servers.WebServer.Abstraction.Sake
{
    public abstract class ResponseBase : Abstraction.ResponseBase
    {
        protected ResponseBase(UniSpyRequestBase request, UniSpyResultBase result) : base(request, result)
        {
        }
    }
}