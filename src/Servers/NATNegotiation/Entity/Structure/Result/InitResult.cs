using NATNegotiation.Abstraction.BaseClass;
using NATNegotiation.Entity.Enumerate;
using System.Net;

namespace NATNegotiation.Entity.Structure.Result
{
    internal class InitResult : NNResultBase
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
