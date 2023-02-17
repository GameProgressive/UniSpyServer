using UniSpy.Server.WebServer.Module.Direct2Game.Entity.Structure;

namespace UniSpy.Server.WebServer.Module.Direct2Game.Abstraction;
public class ResponseBase : UniSpy.Server.WebServer.Abstraction.ResponseBase
{
    public ResponseBase(RequestBase request, ResultBase result) : base(request, result)
    {
        _content = new Direct2GameSoapEnvelope();
    }
}