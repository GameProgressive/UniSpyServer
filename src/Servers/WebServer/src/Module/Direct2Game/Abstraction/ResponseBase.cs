using UniSpyServer.Servers.WebServer.Module.Direct2Game.Entity.Structure;

namespace UniSpyServer.Servers.WebServer.Module.Direct2Game.Abstraction;
public class ResponseBase : UniSpyServer.Servers.WebServer.Abstraction.ResponseBase
{
    public ResponseBase(RequestBase request, ResultBase result) : base(request, result)
    {
        _content = new Direct2GameSoapEnvelope();
    }
}