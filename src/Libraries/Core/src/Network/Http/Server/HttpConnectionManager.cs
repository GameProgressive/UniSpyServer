using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using UniSpy.Server.Core.Abstraction.Interface;
using UniSpy.Server.Core.Events;
using UniSpy.Server.Core.Logging;

namespace UniSpy.Server.Core.Network.Http.Server;

public class HttpConnectionManager : IConnectionManager
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
                var context = Listener.GetContext();
                Task.Run(() =>
                   {

                       var conn = new HttpConnection(context, this);
                       OnInitialization(conn);
                       var raw = context.Request;
                       var request = new HttpRequest(raw);
                       conn.OnReceived(request);
                   });
            }
        });
    }

    public void Dispose()
    {
        Listener.Stop();
    }
}