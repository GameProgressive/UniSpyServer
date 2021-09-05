using NatNegotiation.Abstraction.BaseClass;
using NatNegotiation.Entity.Enumerate;
using System.Net;

namespace NatNegotiation.Entity.Structure.Result
{
    internal class InitResult : ResultBase
    {
        public string LocalIP => ((IPEndPoint)LocalIPEndPoint).Address.ToString();
        public ushort LocalPort => (ushort)((IPEndPoint)LocalIPEndPoint).Port;
        public IPEndPoint LocalIPEndPoint { get; set; }
        public InitResult()
        {
            PacketType = NatPacketType.InitAck;
        }
    }
}
