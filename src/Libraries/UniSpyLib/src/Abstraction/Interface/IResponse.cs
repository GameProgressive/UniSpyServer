namespace UniSpyServer.UniSpyLib.Abstraction.Interface
{
    public interface IResponse
    {
        object SendingBuffer { get; }
        void Build();
    }
}
