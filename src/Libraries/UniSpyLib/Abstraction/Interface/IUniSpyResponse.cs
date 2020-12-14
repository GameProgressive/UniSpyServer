namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyResponse
    {
        object SendingBuffer { get; }
        object ErrorCode { get; }
        public void Build();
    }
}
