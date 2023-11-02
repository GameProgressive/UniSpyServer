using System;
using System.Net;
using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Core.Network.Http.Server;

public class HttpConnectionManager : IConnectionManager, IDisposable
{
    public event OnConnectingEventHandler OnInitialization;
    public HttpListener Listener { get; private set; }
    public HttpConnectionManager(IPEndPoint endPoint)
    {
        Listener = new HttpListener();
        Listener.Prefixes.Add($"http://localhost:{endPoint.Port}/");
    }
    public void Start()
    {
        Listener.Start();
        Task.Run(() =>
        {
            while (true)
            {
                try
                {
                    var context = Listener.GetContext();
                    var raw = context.Request;
                    var request = new HttpRequest(raw);
                    var conn = new HttpConnection(context, this);
                    OnConnecting(conn);
                    conn.OnReceived(request);
                }
                catch (Exception ex)
                {
                    LogWriter.LogError(ex);
                }
            }
        });
    }
    public void OnConnecting(IHttpConnection connection)
    {
        OnInitialization((IConnection)connection);
    }

    public void Dispose()
    {
        Listener.Stop();
    }
}