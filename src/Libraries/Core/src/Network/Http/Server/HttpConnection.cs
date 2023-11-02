using System.Net;
using System.Text;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;

namespace UniSpy.Server.Core.Network.Http.Server;

public class HttpConnection : IHttpConnection
{
    public IConnectionManager Manager { get; private set; }
    public HttpListenerContext Context { get; private set; }
    public IPEndPoint RemoteIPEndPoint { get; private set; }

    public NetworkConnectionType ConnectionType { get; } = NetworkConnectionType.Http;
    public event OnReceivedEventHandler OnReceive;
    public HttpConnection(HttpListenerContext context, IConnectionManager manager)
    {
        Manager = manager;
        Context = context;
        RemoteIPEndPoint = context.Request.RemoteEndPoint;
    }
    public void OnReceived(IHttpRequest request)
    {
        OnReceive(request);
    }

    public void Send(string response)
    {
        Send(Encoding.UTF8.GetBytes(response));
    }

    public void Send(byte[] response)
    {
        Context.Response.StatusCode = (int)HttpStatusCode.OK;
        Context.Response.ContentType = "application/xml?";
        Context.Response.OutputStream.Write(response);
    }
}