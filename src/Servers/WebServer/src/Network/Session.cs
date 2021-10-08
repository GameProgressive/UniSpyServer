using System;
using System.Collections.Generic;
using System.Linq;
using NetCoreServer;
using UniSpyLib.Abstraction.BaseClass.Network.Http.Server;

namespace WebServer.Network
{
    internal class Session : UniSpyHttpSession
    {
        public Session(UniSpyHttpServer server) : base(server)
        {
        }
        private static string SplitMultipart(string body, string boundary)
        {
            var ret = "";

            if (boundary == null)
                throw new ArgumentNullException(nameof(boundary));
            if (body == null)
                throw new ArgumentNullException(nameof(body));

            var data = body.Split("--" + boundary);

            foreach (var datacon in data)
            {
                var trimmedData = datacon.TrimStart(new[] { '\r', '\n' }).TrimEnd(new[] { '\r', '\n' });

                if (trimmedData != "--" && trimmedData.Length > 0)
                {
                    ret += trimmedData;
                }
            }

            return ret;
        }
        protected override void OnReceivedRequest(HttpRequest request)
        {
            var uri = new Uri(request.Url);
            Console.WriteLine($"Web Server {request.Method}:");

            Console.WriteLine($"\tUrl: {request.Url}\n" +
                              $"\tHeaders: {request.Headers}\n" +
                              $"\tCookies: {request.Cookies}\n" +
                              $"\tProtocol: {request.Protocol}\n" +
                              $"\tBody length: {request.BodyLength}"
            );

            if (request.Headers != 0)
            {
                Console.WriteLine("\tHeaders: ");
                for (var i = 0; i < request.Headers; i++)
                {
                    Console.WriteLine($"\t\t{request.Header(i)}");
                }
            }

            if (request.Cookies != 0)
            {
                Console.WriteLine("\tCookies: ");
                for (var i = 0; i < request.Cookies; i++)
                {
                    Console.WriteLine($"\t\t{request.Cookie(i)}");
                }
            }

            var body = "";

            if (request.BodyLength > 0)
            {
                Console.WriteLine($"\tBody:\n\n{request.Body}\n\n");

                for (var i = 0; i < request.Headers; i++)
                {
                    if (request.Header(i).Item1 == "Content-Type")
                    {
                        var contentType = request.Header(i).Item2;
                        if (contentType.Contains("multipart/form-data"))
                        {
                            body = SplitMultipart(request.Body,
                                contentType.Split("; ").FirstOrDefault((str) => str.Contains("boundary="))?.Replace("boundary=", ""));
                        }
                    }
                }

                body ??= request.Body;
            }

            Dictionary<string, string> args = null;
            string[] url = request.Url.TrimStart('/').Split('?');

            if (url.Length > 0)
            {
                args = url[1].Split('&').ToDictionary(x => x[..x.IndexOf('=')], x => x[(x.IndexOf('=') + 1)..].TrimStart(' '));
            }
        }
    }
}