using UniSpyServer.Servers.WebServer.Abstraction;

namespace UniSpyServer.Servers.WebServer.Module.Sake.Abstraction
{
    public abstract class ResponseBase : WebServer.Abstraction.ResponseBase
    {
        protected ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
        }
    }
}