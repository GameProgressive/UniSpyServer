namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IHttpRequest : IRequest
    {
        byte[] BodyBytes { get; }
        string Body { get; }
        long Cookies { get; }
        long Headers { get; }
        string Protocol { get; }
        string Url { get; }
        string Method { get; }
        bool IsErrorSet { get; }
        bool IsEmpty { get; }
        long BodyLength { get; }
    }
}