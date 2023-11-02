using System;
using System.IO;
using System.Net;
using System.Text;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Core.Network.Http.Server;
public class HttpRequest : IHttpRequest
{
    public string Body { get; private set; }
    public Uri Url { get; private set; }
    public string Method { get; private set; }
    public HttpListenerRequest RawRequest { get; private set; }
    public HttpRequest(HttpListenerRequest rawRequest)
    {
        RawRequest = rawRequest;
        Method = RawRequest.HttpMethod;
        Url = RawRequest.Url;
        using (var stream = rawRequest.InputStream)
        {
            using (var reader = new StreamReader(stream, rawRequest.ContentEncoding))
            {
                Body = reader.ReadToEnd();
            }
        }
    }
}