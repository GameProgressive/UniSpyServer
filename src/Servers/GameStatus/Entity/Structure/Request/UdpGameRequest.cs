using GameStatus.Abstraction.BaseClass;

namespace GameStatus.Entity.Structure.Request
{
    internal sealed class UdpGameRequest : GSRequestBase
    {
        public UdpGameRequest(string rawRequest) : base(rawRequest)
        {
        }
    }
}