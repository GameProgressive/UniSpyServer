using System;
using UniSpy.Server.Core.Abstraction.Interface;

namespace UniSpy.Server.Core.Abstraction.BaseClass.Network.Http.Server
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

        public bool KeepAlive { get; private set; }


        public HttpRequest(NetCoreServer.HttpRequest request)
        {
            Url = request.Url;
            BodyBytes = request.BodyBytes;
            Body = request.Body;
            Cookies = request.Cookies;
            Headers = request.Headers;
            Protocol = request.Protocol;
            Method = request.Method;
            KeepAlive = false;

            for (var m = 0; m < request.Headers; m++)
            {
                var k = request.Header(m);
                if (k.Item1 == "Connection" && k.Item2.ToLower() == "keep-alive")
                    KeepAlive = true;
            }
        }
    }
}