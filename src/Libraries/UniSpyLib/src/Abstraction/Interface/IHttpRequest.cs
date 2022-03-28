namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IHttpRequest
    {
        byte[] BodyBytes { get; }
        string Body { get; }
        long Cookies { get; }
        long Headers { get; }
        string Protocol { get; }
        string Url { get; }
        string Method { get; }
        bool KeepAlive { get; }
    }
}