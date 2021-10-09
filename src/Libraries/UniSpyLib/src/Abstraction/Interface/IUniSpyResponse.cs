namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyResponse
    {
        object SendingBuffer { get; }
        void Build();
    }
}
