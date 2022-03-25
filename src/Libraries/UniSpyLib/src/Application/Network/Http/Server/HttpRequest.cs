using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Application.Network.Http.Server
{
    public class HttpRequest : NetCoreServer.HttpRequest, IHttpRequest
    {
        public Uri Uri => new Uri(Url);

        public object CommandName => throw new NotImplementedException();

        public object RawRequest => throw new NotImplementedException();

        public HttpRequest()
        {
        }

        public HttpRequest(string method, string url, string protocol = "HTTP/1.1") : base(method, url, protocol)
        {
        }

        public void Parse()
        {
            throw new NotImplementedException();
        }
    }
}