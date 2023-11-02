using System;

namespace UniSpy.Server.Core.Abstraction.Interface
{
    public interface IHttpRequest
    {
        string Body { get; }
        Uri Url { get; }
        string Method { get; }
    }
}