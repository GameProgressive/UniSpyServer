namespace UniSpy.Server.Core.Abstraction.Interface
{
    public interface IResponse
    {
        object SendingBuffer { get; }
        void Build();
    }
}
