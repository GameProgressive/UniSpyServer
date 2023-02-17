using UniSpy.Server.WebServer.Abstraction;
using UniSpy.Server.WebServer.Module.Auth.Contract;

namespace UniSpy.Server.WebServer.Module.Auth.Abstraction
{
    public abstract class ResponseBase : WebServer.Abstraction.ResponseBase
    {
        protected ResponseBase(RequestBase request, ResultBase result) : base(request, result)
        {
            _content = new AuthSoapEnvelope();
        }
    }
}