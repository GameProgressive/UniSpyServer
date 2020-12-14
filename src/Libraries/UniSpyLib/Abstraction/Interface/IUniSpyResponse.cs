using System;
namespace UniSpyLib.Abstraction.Interface
{
    public interface IUniSpyResponse
    {
        object SendingBuffer { get; }
        public void Build();
    }
}
