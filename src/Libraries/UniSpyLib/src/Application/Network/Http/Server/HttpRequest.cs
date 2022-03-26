using System;
using UniSpyServer.UniSpyLib.Abstraction.Interface;

namespace UniSpyServer.UniSpyLib.Application.Network.Http.Server
{
    public class HttpRequest : IHttpRequest
    {
        public Uri Uri => new Uri(Url);

        public byte[] BodyBytes { get; private set; }

        public string Body { get; private set; }

        public long Cookies { get; private set; }

        public long Headers { get; private set; }

        public string Protocol { get; private set; }

        public string Url { get; private set; }

        public string Method { get; private set; }


        public HttpRequest(NetCoreServer.HttpRequest request)
        {
            Url = request.Url;
            BodyBytes = request.BodyBytes;
            Body = request.Body;
            Cookies = request.Cookies;
            Headers = request.Headers;
            Protocol = request.Protocol;
            Method = request.Method;
        }
    }
}